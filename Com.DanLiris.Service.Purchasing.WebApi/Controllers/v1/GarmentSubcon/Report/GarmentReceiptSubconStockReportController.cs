﻿using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentSubcon.Report.GarmentReceiptSubconStockReport;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.DanLiris.Service.Purchasing.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Purchasing.WebApi.Controllers.v1.GarmentSubcon.Report
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/garment-receipt-subcon-stock-report")]
    [Authorize]
    public class GarmentReceiptSubconStockReportController : Controller
    {
    
        private string ApiVersion = "1.0.0";
        private readonly IGarmentReceiptSubconStockReportFacade _facade;
        private readonly IServiceProvider serviceProvider;
        private readonly IdentityService identityService;

        public GarmentReceiptSubconStockReportController(IGarmentReceiptSubconStockReportFacade facade, IServiceProvider serviceProvider)
        {
            this._facade = facade;
            this.serviceProvider = serviceProvider;
            identityService = (IdentityService)serviceProvider.GetService(typeof(IdentityService));
        }
        [HttpGet]
        public IActionResult GetReportGarmentStock(DateTime? dateFrom, DateTime? dateTo, string category, string unitcode, int page = 1, int size = 25, string Order = "{}")
        {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                identityService.TimezoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
                identityService.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");

                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                string accept = Request.Headers["Accept"];

                var data = _facade.GetStockReport(offset, unitcode, category, page, size, Order, dateFrom, dateTo);



                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Item1,
                    info = new { total = data.Item2 },
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message + "\n" + e.StackTrace)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("download")]
        public IActionResult GetXls(DateTime? dateFrom, DateTime? dateTo, string category, string categoryname, string unitname, string unitcode)
        {

            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                identityService.TimezoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
                identityService.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");

                byte[] xlsInBytes;
                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
              
                MemoryStream xls = _facade.GenerateExcelStockReport(category, categoryname, unitname, unitcode, dateFrom, dateTo, offset);


                string filename = String.IsNullOrWhiteSpace(unitcode) ? String.Format("Laporan Stock Barang Terima Subcon All Unit - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy")) : unitcode == "C2A" ? String.Format("Laporan Stock Barang Terima Subcon KONFEKSI 2A - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy")) : unitcode == "C2B" ? String.Format("Laporan Stock Barang Terima Subcon KONFEKSI 2B - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy")) : unitcode == "C2C" ? String.Format("Laporan Stock Barang Terima Subcon KONFEKSI 2C - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy")) : unitcode == "C1B" ? String.Format("Laporan Stock Barang Terima Subcon KONFEKSI 2D - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy")) : String.Format("Laporan  Stock Barang Terima Subcon KONFEKSI 1 MNS - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

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
