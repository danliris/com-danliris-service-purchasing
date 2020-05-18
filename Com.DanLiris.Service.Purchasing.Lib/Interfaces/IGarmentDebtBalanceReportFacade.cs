using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentReports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Lib.Interfaces
{
    public interface IGarmentDebtBalanceReportFacade
    {
        List<GarmentDebtBalanceFixViewModel> GetReport(bool jnsSpl, string payMtd, string category, DateTime? dateFrom, DateTime? dateTo, string Order, int offset);
        MemoryStream GetXLs(bool jnsSpl, string payMtd, string category, DateTime? dateFrom, DateTime? dateTo, int offset);
    }
}
