using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentInternalPurchaseOrderViewModel
{
    public class GarmentInternalPurchaseOrderReportViewModel
    {
        public long prId { get; set; }
        public DateTimeOffset prDate { get; set; }
        public string prNo { get; set; }
        public DateTimeOffset createdUtc { get; set; }
        public DateTimeOffset shipmentDate { get; set; }
        public string roNo { get; set; }
        public string buyerId { get; set; }
        public string buyerCode { get; set; }
        public string buyerName { get; set; }
        public string article { get; set; }
        public string unitId { get; set; }
        public string unitCode { get; set; }
        public string unitName { get; set; }
        public string po_SerialNumber { get; set; }
        public string categoryId { get; set; }
        public string categoryName { get; set; }
        public string productId { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productRemark { get; set; }
        public double quantity { get; set; }
        public string uomId { get; set; }
        public string uomUnit { get; set; }
        public double budgetPrice { get; set; }
        public virtual string createdBy { get; set; }

        public bool isPosted { get; set; }
        public string remark { get; set; }
        public long gprItemId { get; set; }
        public virtual long gPOId { get; set; }

    }
}
