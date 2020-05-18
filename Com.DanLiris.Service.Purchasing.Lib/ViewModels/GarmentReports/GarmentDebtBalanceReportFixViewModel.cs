using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentReports
{
    public class GarmentDebtBalanceFixViewModel
    {
        public int count { get; set; }
        public DateTimeOffset? ArrivalDate { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string CurrencyCode { get; set; }
        public string DONo { get; set; }
        public string BillNo { get; set; }
        public string CodeRequirement { get; set; }
        public decimal DebtAmount { get; set; }
        public string PaymentNo { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal DebtBalance { get; set; }
    }
}
