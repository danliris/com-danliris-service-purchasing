﻿using Com.DanLiris.Service.Purchasing.Lib.Utilities;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.IntegrationViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.DanLiris.Service.Purchasing.Lib.ViewModels.UnitReceiptNoteViewModel
{
    public class UnitReceiptNoteGenerateDataViewModel 
    {
        public string URNNo { get; set; }
        public DateTimeOffset URNDate { get; set; }
        public string UnitName { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string DONo { get; set; }
        public string URNRemark { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public string UOMUnit { get; set; }
        public string Remark { get; set; }
    }

    public class GarmentStockOpnameViewModel
    {
        public long IdSO { get; set; }
        public string POSerialNumber { get; set; }
        public string RONo { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string DONo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string DesignColor { get; set; }
        public decimal Quantity { get; set; }
        public decimal BeforeQuantity { get; set; }
    }
}