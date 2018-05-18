using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.ViewModels.UnitReceiptNote
{
    public class UnitReceiptNoteViewModel
    {
        public string No { get; set; }
        public DateTime Date { get; set; }
        public List<UnitReceiptNoteItemViewModel> UnitReceiptNoteItems { get; set; }

    }
}
