﻿using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.Helpers;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentDeliveryOrderModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentExternalPurchaseOrderModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentInternalPurchaseOrderModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentInventoryModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentUnitDeliveryOrderModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentUnitExpenditureNoteModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentUnitReceiptNoteModel;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentUnitReceiptNoteFacades
{
    public class GarmentDOItemFacade : IGarmentDOItemFacade
    {
        private readonly string USER_AGENT = "Facade";

        private readonly IServiceProvider serviceProvider;
        private readonly IdentityService identityService;

        private readonly DbSet<GarmentDOItems> dbSetGarmentDOItems;
        private readonly DbSet<GarmentUnitReceiptNote> dbSetGarmentUnitReceiptNote;
        private readonly DbSet<GarmentUnitReceiptNoteItem> dbSetGarmentUnitReceiptNoteItem;
        private readonly DbSet<GarmentExternalPurchaseOrderItem> dbSetGarmentExternalPurchaseOrderItem;

        public GarmentDOItemFacade(IServiceProvider serviceProvider, PurchasingDbContext dbContext)
        {
            this.serviceProvider = serviceProvider;
            identityService = (IdentityService)serviceProvider.GetService(typeof(IdentityService));

            dbSetGarmentDOItems = dbContext.Set<GarmentDOItems>();
            dbSetGarmentUnitReceiptNote = dbContext.Set<GarmentUnitReceiptNote>();
            dbSetGarmentUnitReceiptNoteItem = dbContext.Set<GarmentUnitReceiptNoteItem>();
            dbSetGarmentExternalPurchaseOrderItem = dbContext.Set<GarmentExternalPurchaseOrderItem>();
        }

        public List<object> ReadForUnitDO(string Keyword = null, string Filter = "{}")
        {
            IQueryable<GarmentDOItems> GarmentDOItemsQuery = dbSetGarmentDOItems;
            IQueryable<GarmentUnitReceiptNoteItem> GarmentUnitReceiptNoteItemsQuery = dbSetGarmentUnitReceiptNoteItem;
            IQueryable<GarmentUnitReceiptNote> GarmentUnitReceiptNotesQuery = dbSetGarmentUnitReceiptNote;
            //IQueryable<GarmentExternalPurchaseOrderItem> GarmentExternalPurchaseOrderItemsQuery = dbSetGarmentExternalPurchaseOrderItem;

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            long unitId = 0;
            long storageId = 0;
            bool hasUnitFilter = FilterDictionary.ContainsKey("UnitId") && long.TryParse(FilterDictionary["UnitId"], out unitId);
            bool hasStorageFilter = FilterDictionary.ContainsKey("StorageId") && long.TryParse(FilterDictionary["StorageId"], out storageId);
            bool hasRONoFilter = FilterDictionary.ContainsKey("RONo");
            string RONo = hasRONoFilter ? (FilterDictionary["RONo"] ?? "").Trim() : "";

            if (hasUnitFilter)
            {
                GarmentDOItemsQuery = GarmentDOItemsQuery.Where(x => x.UnitId == unitId);
            }
            if (hasStorageFilter)
            {
                GarmentDOItemsQuery = GarmentDOItemsQuery.Where(x => x.StorageId == storageId);
            }
            if (hasRONoFilter)
            {
                GarmentDOItemsQuery = GarmentDOItemsQuery.Where(x => x.RO == RONo);
            }

            List<object> ListData = new List<object>();
            var data = from doi in GarmentDOItemsQuery
                       join urni in GarmentUnitReceiptNoteItemsQuery on doi.URNItemId equals urni.Id
                       join urn in GarmentUnitReceiptNotesQuery on urni.URNId equals urn.Id
                       //join epoi in GarmentExternalPurchaseOrderItemsQuery on doi.EPOItemId equals epoi.Id
                       select new
                       {
                           DOItemsId = doi.Id,
                           urni.URNId,
                           doi.URNItemId,
                           doi.EPOItemId,
                       };
            var urnIds = data.Select(s => s.URNId).Distinct().ToList();
            var URNs = dbSetGarmentUnitReceiptNote.Where(u => urnIds.Contains(u.Id))
                .Select(s => new { s.Id, s.URNNo }).ToList();
            var urnItemIds = data.Select(s => s.URNItemId).Distinct().ToList();
            var urnItems = dbSetGarmentUnitReceiptNoteItem.Where(w => urnItemIds.Contains(w.Id))
                .Select(s => new { s.Id, s.DODetailId, s.ProductRemark, s.PricePerDealUnit, s.ReceiptCorrection, s.CorrectionConversion }).ToList();
            var epoItemIds = data.Select(s => s.EPOItemId).Distinct().ToList();
            var epoItems = dbSetGarmentExternalPurchaseOrderItem.Where(w => epoItemIds.Contains(w.Id))
                .Select(s => new { s.Id, s.Article }).ToList();
            var DOItemIds = data.Select(s => s.DOItemsId).Distinct().ToList();
            var DOItems = dbSetGarmentDOItems.Where(w => DOItemIds.Contains(w.Id))
                .Select(s => new
                {
                    DOItemsId = s.Id,
                    s.POItemId,
                    s.URNItemId,
                    s.EPOItemId,
                    s.PRItemId,
                    s.ProductId,
                    s.ProductCode,
                    s.ProductName,
                    s.SmallQuantity,
                    s.SmallUomId,
                    s.SmallUomUnit,
                    s.DesignColor,
                    s.POSerialNumber,
                    s.RemainingQuantity,
                    RONo = s.RO
                }).ToList();
            foreach (var item in data)
            {
                var urn = URNs.FirstOrDefault(f => f.Id.Equals(item.URNId));
                var urnItem = urnItems.FirstOrDefault(f => f.Id.Equals(item.URNItemId));
                var epoItem = epoItems.FirstOrDefault(f => f.Id.Equals(item.EPOItemId));
                var doItem = DOItems.FirstOrDefault(f => f.DOItemsId.Equals(item.DOItemsId));

                ListData.Add(new
                {
                    DOItemId = doItem.DOItemsId,
                    URNId = urn.Id,
                    urn.URNNo,
                    doItem.POItemId,
                    doItem.URNItemId,
                    doItem.EPOItemId,
                    doItem.PRItemId,
                    doItem.ProductId,
                    doItem.ProductCode,
                    doItem.ProductName,
                    doItem.SmallQuantity,
                    doItem.SmallUomId,
                    doItem.SmallUomUnit,
                    doItem.DesignColor,
                    doItem.POSerialNumber,
                    doItem.RemainingQuantity,
                    doItem.RONo,
                    epoItem.Article,
                    urnItem.DODetailId,
                    urnItem.ProductRemark,
                    urnItem.PricePerDealUnit,
                    urnItem.ReceiptCorrection,
                    urnItem.CorrectionConversion
                });
            }

            return ListData;
        }

        public List<object> ReadForUnitDOMore(string Keyword = null, string Filter = "{}", int size = 50)
        {
            IQueryable<GarmentDOItems> GarmentDOItemsQuery = dbSetGarmentDOItems.Where(w => w.IsDeleted == false);
            IQueryable<GarmentUnitReceiptNoteItem> GarmentUnitReceiptNoteItemsQuery = dbSetGarmentUnitReceiptNoteItem.Where(w => w.IsDeleted == false);
            IQueryable<GarmentUnitReceiptNote> GarmentUnitReceiptNotesQuery = dbSetGarmentUnitReceiptNote.Where(w => w.IsDeleted == false);
            IQueryable<GarmentExternalPurchaseOrderItem> GarmentExternalPurchaseOrderItemsQuery = dbSetGarmentExternalPurchaseOrderItem.IgnoreQueryFilters();

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            long unitId = 0;
            long storageId = 0;
            bool hasUnitFilter = FilterDictionary.ContainsKey("UnitId") && long.TryParse(FilterDictionary["UnitId"], out unitId);
            bool hasStorageFilter = FilterDictionary.ContainsKey("StorageId") && long.TryParse(FilterDictionary["StorageId"], out storageId);
            bool hasRONoFilter = FilterDictionary.ContainsKey("RONo");
            string RONo = hasRONoFilter ? (FilterDictionary["RONo"] ?? "").Trim() : "";

            if (hasUnitFilter)
            {
                GarmentDOItemsQuery = GarmentDOItemsQuery.Where(x => x.UnitId == unitId);
            }
            if (hasStorageFilter)
            {
                GarmentDOItemsQuery = GarmentDOItemsQuery.Where(x => x.StorageId == storageId);
            }
            if (hasRONoFilter)
            {
                GarmentDOItemsQuery = GarmentDOItemsQuery.Where(x => x.RO != RONo);
            }

            Keyword = (Keyword ?? "").Trim();
            GarmentDOItemsQuery = GarmentDOItemsQuery.Where(x => x.RemainingQuantity > 0 && (x.RO.Contains(Keyword) || x.POSerialNumber.Contains(Keyword)));

            var data = from doi in GarmentDOItemsQuery
                           //join urni in GarmentUnitReceiptNoteItemsQuery on doi.URNItemId equals urni.Id
                           //join urn in GarmentUnitReceiptNotesQuery on urni.URNId equals urn.Id
                           //join epoi in GarmentExternalPurchaseOrderItemsQuery on doi.EPOItemId equals epoi.Id
                       select new
                       {
                           //DOItemsId = doi.Id,
                           //urn.URNNo,
                           //urni.URNId,
                           doi.URNItemId,
                           RONo = doi.RO,
                           //urni.DODetailId,
                           //doi.EPOItemId,
                           //doi.POItemId,
                           //doi.PRItemId,
                           //doi.ProductId,
                           doi.ProductName,
                           doi.ProductCode,
                           //urni.ProductRemark,
                           //doi.SmallQuantity,
                           //doi.DesignColor,
                           //doi.SmallUomId,
                           //doi.SmallUomUnit,
                           doi.POSerialNumber,
                           //urni.PricePerDealUnit,
                           //urni.ReceiptCorrection,
                           //urni.Conversion,
                           //urni.CorrectionConversion,
                           //epoi.Article,
                           //doi.RemainingQuantity
                       };

            List<object> ListData = new List<object>(data.OrderBy(o => o.RONo).Take(size));
            return ListData;
        }
    }
}
