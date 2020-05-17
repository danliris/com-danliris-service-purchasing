using Com.DanLiris.Service.Purchasing.Lib.Helpers;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentInternNoteModel;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentReports;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentReports
{
    public class GarmentDebtBalanceReportFacade : IGarmentDebtBalanceReportFacade
    {
        private readonly PurchasingDbContext dbContext;
        ILocalDbCashFlowDbContext dbContextLocal;
        public readonly IServiceProvider serviceProvider;
        private readonly DbSet<GarmentInternNote> dbSet;

        public GarmentDebtBalanceReportFacade(IServiceProvider serviceProvider, PurchasingDbContext dbContext, ILocalDbCashFlowDbContext dbContextLocal)
        {
            this.serviceProvider = serviceProvider;
            this.dbContext = dbContext;
            this.dbContextLocal = dbContextLocal;
            this.dbSet = dbContext.Set<GarmentInternNote>();
        }

        public IQueryable<GarmentDebtBalanceFixViewModel> GetQuery(bool jnsSpl, string payMtd, string category, DateTime? dateFrom, DateTime? dateTo, int offset)
        {
            DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
            DateTime DateTo = dateTo == null ? DateTime.Now : (DateTime)dateTo;
            List<GarmentDebtBalanceViewModel> DebtBalance = new List<GarmentDebtBalanceViewModel>();
            List<GarmentDebtBalanceFixViewModel> DebtBalance1 = new List<GarmentDebtBalanceFixViewModel>();


            var Query = (from a in dbContext.GarmentDeliveryOrders 
                        join b in dbContext.GarmentDeliveryOrderItems on a.Id equals b.GarmentDOId
                        join c in dbContext.GarmentDeliveryOrderDetails on b.Id equals c.GarmentDOItemId
                        join d in dbContext.GarmentExternalPurchaseOrders on b.EPOId equals d.Id
                        where a.IsDeleted == false && b.IsDeleted == false && c.IsDeleted == false && d.IsDeleted == false  
                        && d.SupplierImport == jnsSpl
                        && d.PaymentMethod == (string.IsNullOrWhiteSpace(payMtd) ? d.PaymentMethod : payMtd)
                        && c.CodeRequirment == (string.IsNullOrWhiteSpace(category) ? c.CodeRequirment : category)
                        && a.ArrivalDate.AddHours(offset).Date >= DateFrom.Date
                        && a.ArrivalDate.AddHours(offset).Date <= DateTo.Date

                        select new GarmentDebtBalanceViewModel
                        {
                            ArrivalDate = a.ArrivalDate,
                            SupplierCode = a.SupplierCode,
                            SupplierName = a.SupplierName,
                            CurrencyCode = a.DOCurrencyCode,
                            DONo = a.DONo,
                            BillNo = a.BillNo,
                            //PaymentBill = a.PaymentBill,
                            CodeRequirement = c.CodeRequirment,
                            DebtAmount = (decimal)c.PriceTotal,
                            PaymentNo = "",
                            PaymentDate = new DateTime(1970, 1, 1), 
                            PaymentAmount = 0,
                            DebtBalance = 0,
                        });

            var Data = from a in Query
                       group new { Amt = a.DebtAmount } by new { a.ArrivalDate, a.SupplierCode, a.SupplierName, a.CurrencyCode, a.BillNo, a.DONo, a.CodeRequirement } into G
                       select new GarmentDebtBalanceViewModel
                       {
                           ArrivalDate = G.Key.ArrivalDate,
                           SupplierCode = G.Key.SupplierCode,
                           SupplierName = G.Key.SupplierName,
                           CurrencyCode = G.Key.CurrencyCode,
                           DONo = G.Key.DONo,
                           BillNo = G.Key.BillNo,
                           CodeRequirement = G.Key.CodeRequirement,
                           DebtAmount = Math.Round(G.Sum(m => m.Amt), 2),
                           PaymentNo = "",
                           PaymentDate = new DateTime(1970, 1, 1),
                           PaymentAmount = 0,
                           DebtBalance = 0
                       };

            foreach (GarmentDebtBalanceViewModel i in Data)
            {
                string cmddetail = "Select nomor,tgl,jumlah from RincianDetil where no_bon = '" + i.BillNo + "' and no_sj = '" + i.DONo + "'";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("bon", i.BillNo));
                parameters.Add(new SqlParameter("do", i.DONo));

                var data = dbContextLocal.ExecuteReaderOnlyQuery(cmddetail);
                
                while (data.Read())
                {
                    i.PaymentNo = data["nomor"].ToString();
                    i.PaymentDate = data.GetDateTime(1);
                    i.PaymentAmount = (decimal)data["jumlah"];
                 };

                DebtBalance.Add(i);

                DebtBalance1.Add(new GarmentDebtBalanceFixViewModel
                {
                    ArrivalDate = i.ArrivalDate,
                    SupplierCode = i.SupplierCode,
                    SupplierName = i.SupplierName,
                    CurrencyCode = i.CurrencyCode,
                    DONo = i.DONo,
                    BillNo = i.BillNo,
                    CodeRequirement = i.CodeRequirement,
                    DebtAmount = i.DebtAmount,
                    PaymentNo = i.PaymentNo,
                    PaymentDate = i.PaymentDate,
                    PaymentAmount = i.PaymentAmount,
                    DebtBalance = i.DebtAmount - i.PaymentAmount,
                });
            };
      
            return DebtBalance1.Distinct().AsQueryable();
           
        }
        public List<GarmentDebtBalanceFixViewModel> GetReport(bool jnsSpl, string payMtd, string category, DateTime? dateFrom, DateTime? dateTo, string Order, int offset)
        {
            var Query = GetQuery(jnsSpl, payMtd, category, dateFrom, dateTo, offset);
            Query.OrderBy(x => x.SupplierCode).ThenBy(x => x.CurrencyCode).ThenBy(x => x.BillNo).ThenBy(x => x.DONo);            
            return Query.ToList();
        }

        public MemoryStream GetXLs(bool jnsSpl, string payMtd, string category, DateTime? dateFrom, DateTime? dateTo, int offset)
        {
            var Data = GetQuery(jnsSpl, payMtd, category, dateFrom, dateTo, offset);
            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Kode Supplier", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nama Supplier", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Mata Uang", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nomor Nota / Nomor Bon Pusat", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Nota", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No.Surat Jalan", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tipe", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Jumlah Hutang", DataType = typeof(decimal) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nomor Kasbon", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Kasbon", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "Jumlah Bayar", DataType = typeof(decimal) });
            result.Columns.Add(new DataColumn() { ColumnName = "Saldo Hutang", DataType = typeof(decimal) });

            if (Data.ToArray().Count() == 0)
                result.Rows.Add("", "", "", "", "", "", "", "", 0, "","", 0, 0);
            else
            {
                int index = 0;
                foreach (var item in Data)
                {
                    index++;
                    string ArvlDate = item.ArrivalDate == new DateTime(1970, 1, 1) || item.ArrivalDate.Value.ToString("dd MMM yyyy") == "01 Jan 0001" ? "-" : item.ArrivalDate.Value.ToOffset(new TimeSpan(offset, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    string PayDate = item.PaymentDate == new DateTime(1970, 1, 1) || item.PaymentDate.Value.ToString("dd MMM yyyy") == "01 Jan 0001" ? "-" : item.PaymentDate.Value.ToOffset(new TimeSpan(offset, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));

                    result.Rows.Add(index, item.SupplierCode, item.SupplierName, item.CurrencyCode, item.BillNo, ArvlDate, item.DONo, item.CodeRequirement, (Decimal)Math.Round((item.DebtAmount), 2), item.PaymentNo, PayDate, (Decimal)Math.Round((item.PaymentAmount), 2), (Decimal)Math.Round((item.DebtBalance), 2));
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Report Saldo Hutang") }, true);
        }
    }
}
