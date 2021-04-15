using Com.DanLiris.Service.Purchasing.Lib.Enums;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentPurchasingExpedition
{
    public class UpdatePositionDeliveryOrderFormDto
    {
        public List<string> InternalNoteNos { get; set; }
        public PurchasingGarmentExpeditionPosition Position { get; set; }
    }
}