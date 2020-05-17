using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.DanLiris.Service.Purchasing.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Purchasing.WebApi.Controllers.v1.GarmentReports
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/debt-balance-reports")]
    [Authorize]
    public class GarmentDebtBalanceReportController : Controller
    {
        private string ApiVersion = "1.0.0";
        private readonly IMapper mapper;
        private readonly IGarmentDebtBalanceReportFacade _facade;
        private readonly IServiceProvider serviceProvider;
        private readonly IdentityService identityService;

        public GarmentDebtBalanceReportController(IGarmentDebtBalanceReportFacade facade, IServiceProvider serviceProvider)
        {
            this._facade = facade;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        public IActionResult GetReport(bool jnsSpl, string payMtd, string category, DateTime? dateFrom, DateTime? dateTo, string Order = "{}")
        {
            try
            {
                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                string accept = Request.Headers["Accept"];

                var data = _facade.GetReport(jnsSpl, payMtd, category, dateFrom, dateTo, Order, offset);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data,
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
        [HttpGet("download")]
        public IActionResult GetXlsPayment(bool jnsSpl, string payMtd, string category, DateTime? dateFrom, DateTime? dateTo, string Order = "{}")
        {
            try
            {
                byte[] xlsInBytes;
                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var xls = _facade.GetXLs(jnsSpl, payMtd, category, dateFrom, dateTo, offset);

                string filename = String.Format("Laporan Saldo Hutang - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}
