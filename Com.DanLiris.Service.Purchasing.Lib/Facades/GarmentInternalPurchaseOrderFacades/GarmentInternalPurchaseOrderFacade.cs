using Com.DanLiris.Service.Purchasing.Lib.Helpers;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentExternalPurchaseOrderModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentInternalPurchaseOrderModel;
using Com.DanLiris.Service.Purchasing.Lib.Utilities;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentInternalPurchaseOrderFacades
{
    public class GarmentInternalPurchaseOrderFacade : IGarmentInternalPurchaseOrderFacade
    {
        private string USER_AGENT = "Facade";

        private readonly PurchasingDbContext dbContext;
        private readonly DbSet<GarmentInternalPurchaseOrder> dbSet;

        public GarmentInternalPurchaseOrderFacade(PurchasingDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<GarmentInternalPurchaseOrder>();
        }

        public Tuple<List<GarmentInternalPurchaseOrder>, int, Dictionary<string, string>> Read(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}")
        {
            IQueryable<GarmentInternalPurchaseOrder> Query = this.dbSet.Include(m => m.Items);

            List<string> searchAttributes = new List<string>()
            {
                "PRNo", "RONo", "BuyerName", "Items.ProductName", "Items.UomUnit", "CreatedBy"
            };

            Query = QueryHelper<GarmentInternalPurchaseOrder>.ConfigureSearch(Query, searchAttributes, Keyword);

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            Query = QueryHelper<GarmentInternalPurchaseOrder>.ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = QueryHelper<GarmentInternalPurchaseOrder>.ConfigureOrder(Query, OrderDictionary);
            //Query = Query
            //    .Select(m => new
            //    {
            //        Data = m,
            //        ProductName = m.Items.Select(i => i.ProductName).FirstOrDefault()
            //    })
            //    .OrderByDescending(m => m.ProductName)
            //    .AsEnumerable()
            //    .Select(m => m.Data)
            //    .AsQueryable();

            Pageable<GarmentInternalPurchaseOrder> pageable = new Pageable<GarmentInternalPurchaseOrder>(Query, Page - 1, Size);
            List<GarmentInternalPurchaseOrder> Data = pageable.Data.ToList();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary);
        }

        public GarmentInternalPurchaseOrder ReadById(int id)
        {
            var model = dbSet.Where(m => m.Id == id)
                .Include(m => m.Items)
                .FirstOrDefault();
            return model;
        }

        public async Task<int> CreateMultiple(List<GarmentInternalPurchaseOrder> ListModel, string user, int clientTimeZoneOffset = 7)
        {
            int Created = 0;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var model in ListModel)
                    {
                        EntityExtension.FlagForCreate(model, user, USER_AGENT);

                        do
                        {
                            model.PONo = CodeGenerator.Generate();
                        }
                        while (ListModel.Count(m => m.PONo == model.PONo) > 1 || dbSet.Any(m => m.PONo.Equals(model.PONo)));
                        model.IsPosted = false;
                        model.IsClosed = false;

                        foreach (var item in model.Items)
                        {
                            EntityExtension.FlagForCreate(item, user, USER_AGENT);

                            item.Status = "PO Internal belum diorder";

                            var garmentPurchaseRequestItem = dbContext.GarmentPurchaseRequestItems.FirstOrDefault(i => i.Id == item.GPRItemId);
                            garmentPurchaseRequestItem.Status = "Sudah diterima Pembelian";
                            EntityExtension.FlagForUpdate(garmentPurchaseRequestItem, user, USER_AGENT);
                        }

                        dbSet.Add(model);
                    }

                    Created = await dbContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }

            return Created;
        }

        public List<GarmentInternalPurchaseOrder> ReadByTags(string category, string tags, DateTimeOffset shipmentDateFrom, DateTimeOffset shipmentDateTo)
        {
            IQueryable<GarmentInternalPurchaseOrder> Models = this.dbSet.AsQueryable();

            if (shipmentDateFrom != DateTimeOffset.MinValue && shipmentDateTo != DateTimeOffset.MinValue)
            {
                Models = Models.Where(m => m.ShipmentDate >= shipmentDateFrom && m.ShipmentDate <= shipmentDateTo);
            }

            string[] stringKeywords = new string[3];

            if (tags != null)
            {
                List<string> Keywords = new List<string>();

                if (tags.Contains("#"))
                {
                    Keywords = tags.Split("#").ToList();
                    Keywords.RemoveAt(0);
                    Keywords = Keywords.Take(stringKeywords.Length).ToList();
                }
                else
                {
                    Keywords.Add(tags);
                }

                for (int n = 0; n < Keywords.Count; n++)
                {
                    stringKeywords[n] = Keywords[n].Trim().ToLower();
                }
            }

            Models = Models
                .Where(m =>
                    (string.IsNullOrWhiteSpace(stringKeywords[0]) || m.UnitName.ToLower().Contains(stringKeywords[0])) &&
                    (string.IsNullOrWhiteSpace(stringKeywords[1]) || m.BuyerName.ToLower().Contains(stringKeywords[1])) &&
                    //m.Items.Any(i => i.IsUsed == false) &&
                    m.IsPosted == false
                    )
                .Select(m => new GarmentInternalPurchaseOrder
                {
                    Id=m.Id,
                    PONo=m.PONo,
                    PRDate = m.PRDate,
                    PRNo = m.PRNo,
                    RONo = m.RONo,
                    BuyerId = m.BuyerId,
                    BuyerCode = m.BuyerCode,
                    BuyerName = m.BuyerName,
                    Article = m.Article,
                    ExpectedDeliveryDate = m.ExpectedDeliveryDate,
                    ShipmentDate = m.ShipmentDate,
                    UnitId = m.UnitId,
                    UnitCode = m.UnitCode,
                    UnitName = m.UnitName,
                    
                    Items = m.Items
                        .Where(i =>
                            //i.IsPosted == false &&
                            (string.IsNullOrWhiteSpace(stringKeywords[2]) || i.CategoryName.ToLower().Contains(stringKeywords[2]))
                            )
                        .ToList()
                })
                .Where(m => m.Items.Count > 0);

            //var EPOModels = new List<GarmentExternalPurchaseOrderItem>();

            //foreach (var model in Models)
            //{
            //    foreach (var item in model.Items)
            //    {
            //        var EPOModel = new GarmentExternalPurchaseOrderItem
            //        {
            //            PRNo = model.PRNo,
            //            RONo = model.RONo,
            //            De
            //            //IsPosted = false,
            //            //IsClosed = false,
            //            //Remark = "",
            //            Items = new List<GarmentInternalPurchaseOrderItem>
            //            {
            //                new GarmentInternalPurchaseOrderItem
            //                {
            //                    GPRItemId = item.Id,
            //                    PO_SerialNumber = item.PO_SerialNumber,
            //                    ProductId = item.ProductId,
            //                    ProductCode = item.ProductCode,
            //                    ProductName = item.ProductName,
            //                    Quantity = item.Quantity,
            //                    BudgetPrice = item.BudgetPrice,
            //                    UomId = item.UomId,
            //                    UomUnit = item.UomUnit,
            //                    CategoryId = item.CategoryId,
            //                    CategoryName = item.CategoryName,
            //                    ProductRemark = item.ProductRemark,
            //                    //Status = "PO Internal belum diorder"
            //                }
            //            }
            //        };
            //        IPOModels.Add(IPOModel);
            //    }
            //}

            return Models.ToList();
        }
    }
}
