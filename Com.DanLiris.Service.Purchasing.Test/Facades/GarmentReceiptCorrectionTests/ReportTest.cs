using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentCorrectionNoteFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentDeliveryOrderFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentExternalPurchaseOrderFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentInternalPurchaseOrderFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentPurchaseRequestFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentReceiptCorrectionFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentUnitReceiptNoteFacades;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentUnitReceiptNoteModel;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentReceiptCorrectionViewModels;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.NewIntegrationViewModel;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentCorrectionNoteDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentDeliveryOrderDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentExternalPurchaseOrderDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentInternalPurchaseOrderDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentPurchaseRequestDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentReceiptCorrectionDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentUnitReceiptNoteDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.NewIntegrationDataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.DanLiris.Service.Purchasing.Test.Facades.GarmentReceiptCorrectionTests
{
    public class ReportTest
    {

        private const string ENTITY = "GarmentReceiptCorrection";
        private const string USERNAME = "unit-test";
        private IServiceProvider ServiceProvider { get; set; }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private PurchasingDbContext _dbContext(string testName)
        {
            DbContextOptionsBuilder<PurchasingDbContext> optionsBuilder = new DbContextOptionsBuilder<PurchasingDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            PurchasingDbContext dbContext = new PurchasingDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            message.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{\"Id\":7,\"code\":\"USD\",\"rate\":13700.0,\"date\":\"2018/10/20\"}],\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":2,\"order\":{\"date\":\"desc\"},\"select\":[\"Id\",\"code\",\"rate\",\"date\"]}}");
            var HttpClientService = new Mock<IHttpClientService>();
            HttpClientService
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(message);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(HttpClientService.Object);

            return serviceProvider;
        }

       

        private GarmentReceiptCorrectionDataUtil dataUtil(GarmentReceiptCorrectionFacade facade, string testName)
        {
            var garmentPurchaseRequestFacade = new GarmentPurchaseRequestFacade(ServiceProvider, _dbContext(testName));
            var garmentPurchaseRequestDataUtil = new GarmentPurchaseRequestDataUtil(garmentPurchaseRequestFacade);

            var garmentInternalPurchaseOrderFacade = new GarmentInternalPurchaseOrderFacade(_dbContext(testName));
            var garmentInternalPurchaseOrderDataUtil = new GarmentInternalPurchaseOrderDataUtil(garmentInternalPurchaseOrderFacade, garmentPurchaseRequestDataUtil);

            var garmentExternalPurchaseOrderFacade = new GarmentExternalPurchaseOrderFacade(ServiceProvider, _dbContext(testName));
            var garmentExternalPurchaseOrderDataUtil = new GarmentExternalPurchaseOrderDataUtil(garmentExternalPurchaseOrderFacade, garmentInternalPurchaseOrderDataUtil);

            var garmentDeliveryOrderFacade = new GarmentDeliveryOrderFacade(ServiceProvider, _dbContext(testName));
            var garmentDeliveryOrderDataUtil = new GarmentDeliveryOrderDataUtil(garmentDeliveryOrderFacade, garmentExternalPurchaseOrderDataUtil);

            var garmentUnitReceiptNoteFacade = new GarmentUnitReceiptNoteFacade(ServiceProvider, _dbContext(testName));
            var garmentUnitReceiptNoteDataUtil = new GarmentUnitReceiptNoteDataUtil(garmentUnitReceiptNoteFacade, garmentDeliveryOrderDataUtil);

            var garmentCorrectionNoteFacade = new GarmentCorrectionNotePriceFacade(ServiceProvider, _dbContext(testName));
            var garmentCorrectionNoteDataUtil = new GarmentCorrectionNoteDataUtil(garmentCorrectionNoteFacade, garmentDeliveryOrderDataUtil);

            return new GarmentReceiptCorrectionDataUtil(facade, garmentUnitReceiptNoteDataUtil);
        }

        private GarmentDeliveryOrderDataUtil _dataUtilDO(GarmentDeliveryOrderFacade facade, string testName)
        {
            var garmentPurchaseRequestFacade = new GarmentPurchaseRequestFacade(ServiceProvider, _dbContext(testName));
            var garmentPurchaseRequestDataUtil = new GarmentPurchaseRequestDataUtil(garmentPurchaseRequestFacade);

            var garmentInternalPurchaseOrderFacade = new GarmentInternalPurchaseOrderFacade(_dbContext(testName));
            var garmentInternalPurchaseOrderDataUtil = new GarmentInternalPurchaseOrderDataUtil(garmentInternalPurchaseOrderFacade, garmentPurchaseRequestDataUtil);

            var garmentExternalPurchaseOrderFacade = new GarmentExternalPurchaseOrderFacade(ServiceProvider, _dbContext(testName));
            var garmentExternalPurchaseOrderDataUtil = new GarmentExternalPurchaseOrderDataUtil(garmentExternalPurchaseOrderFacade, garmentInternalPurchaseOrderDataUtil);

            return new GarmentDeliveryOrderDataUtil(facade, garmentExternalPurchaseOrderDataUtil);
        }

        [Fact]
        public async Task Should_Success_Get_All_Data()
        {
            var httpClientService = new Mock<IHttpClientService>();
            httpClientService.Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("master/garment-suppliers"))))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new SupplierDataUtil().GetResultFormatterOkString()) });

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService { Username = "Username", TimezoneOffset = 7 });
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);


            var serviceProvider = GetServiceProvider().Object;
            var dbContext = _dbContext(GetCurrentMethod());
            var Facade = new GarmentReceiptCorrectionFacade(_dbContext(GetCurrentMethod()),serviceProviderMock.Object);

           
            
            GarmentDeliveryOrderFacade facadeDO = new GarmentDeliveryOrderFacade(serviceProvider, dbContext);
            var dataUtilDO = _dataUtilDO(facadeDO, GetCurrentMethod());

            var FacadeCorrection = new GarmentCorrectionNotePriceFacade(serviceProviderMock.Object, dbContext);
            var dataUtilCorrection = new GarmentCorrectionNoteDataUtil(FacadeCorrection, dataUtilDO);

            var FacadeUnitReceipt= new GarmentUnitReceiptNoteFacade(serviceProvider, dbContext);
            var dataUtilUnitReceipt = new GarmentUnitReceiptNoteDataUtil(FacadeUnitReceipt, dataUtilDO);

            var dataUtilReceiptCorr = new GarmentReceiptCorrectionDataUtil(Facade, dataUtilUnitReceipt);

            var dataDO = await dataUtilDO.GetNewData();
            await facadeDO.Create(dataDO, USERNAME);

            var dataCorr = await dataUtilCorrection.GetTestData2(dataDO);
            long nowTicks = DateTimeOffset.Now.Ticks;

            var dataUnit = await dataUtilUnitReceipt.GetNewData(nowTicks,dataDO);
            await FacadeUnitReceipt.Create(dataUnit);
            var dataReceipt = await dataUtilReceiptCorr.GetNewData(dataUnit);
            await Facade.Create(dataReceipt.GarmentReceiptCorrection, "unit-test");

            var dateFrom = DateTimeOffset.MinValue;
            var dateTo = DateTimeOffset.UtcNow;
            var facade1 = new GarmentReceiptCorrectionReportFacade(_dbContext(GetCurrentMethod()), serviceProviderMock.Object);

            var Response = facade1.GetReport(null,null,dateFrom,dateTo,"{}",1,25);
            //var garmentReceiptCorrectionFacade = new GarmentReceiptCorrectionFacade(_dbContext(GetCurrentMethod()),GetServiceProvider() );
            // var dataUtilReceiptNote = await dataUtil(Facade, GetCurrentMethod()).GetTestData();

            Assert.NotNull(Response.Item1);
        }

        [Fact]
        public async Task Should_Success_Get_Excel()
        {
            var httpClientService = new Mock<IHttpClientService>();
            httpClientService.Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("master/garment-suppliers"))))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new SupplierDataUtil().GetResultFormatterOkString()) });

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService { Username = "Username", TimezoneOffset = 7 });
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);


            var serviceProvider = GetServiceProvider().Object;
            var dbContext = _dbContext(GetCurrentMethod());
            var Facade = new GarmentReceiptCorrectionFacade(_dbContext(GetCurrentMethod()), serviceProviderMock.Object);



            GarmentDeliveryOrderFacade facadeDO = new GarmentDeliveryOrderFacade(serviceProvider, dbContext);
            var dataUtilDO = _dataUtilDO(facadeDO, GetCurrentMethod());

            var FacadeCorrection = new GarmentCorrectionNotePriceFacade(serviceProviderMock.Object, dbContext);
            var dataUtilCorrection = new GarmentCorrectionNoteDataUtil(FacadeCorrection, dataUtilDO);

            var FacadeUnitReceipt = new GarmentUnitReceiptNoteFacade(serviceProvider, dbContext);
            var dataUtilUnitReceipt = new GarmentUnitReceiptNoteDataUtil(FacadeUnitReceipt, dataUtilDO);

            var dataUtilReceiptCorr = new GarmentReceiptCorrectionDataUtil(Facade, dataUtilUnitReceipt);

            var dataDO = await dataUtilDO.GetNewData();
            await facadeDO.Create(dataDO, USERNAME);

            var dataCorr = await dataUtilCorrection.GetTestData2(dataDO);
            long nowTicks = DateTimeOffset.Now.Ticks;

            var dataUnit = await dataUtilUnitReceipt.GetNewData(nowTicks, dataDO);
            await FacadeUnitReceipt.Create(dataUnit);
            var dataReceipt = await dataUtilReceiptCorr.GetNewData(dataUnit);
            await Facade.Create(dataReceipt.GarmentReceiptCorrection, "unit-test");

            var dateFrom = DateTimeOffset.MinValue;
            var dateTo = DateTimeOffset.UtcNow;
            var facade1 = new GarmentReceiptCorrectionReportFacade(_dbContext(GetCurrentMethod()), serviceProviderMock.Object);

            var Response = facade1.GenerateExcel(null, null, dateFrom, dateTo, "{}");
            //var garmentReceiptCorrectionFacade = new GarmentReceiptCorrectionFacade(_dbContext(GetCurrentMethod()),GetServiceProvider() );
            // var dataUtilReceiptNote = await dataUtil(Facade, GetCurrentMethod()).GetTestData();

            Assert.NotNull(Response);
        }



    }
}
