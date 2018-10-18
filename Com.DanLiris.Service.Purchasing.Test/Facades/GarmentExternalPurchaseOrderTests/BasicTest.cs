using Com.DanLiris.Service.Purchasing.Lib;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentExternalPurchaseOrderFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentInternalPurchaseOrderFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentPurchaseRequestFacades;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentExternalPurchaseOrderViewModel;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentExternalPurchaseOrderDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentInternalPurchaseOrderDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentPurchaseRequestDataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace Com.DanLiris.Service.Purchasing.Test.Facades.GarmentExternalPurchaseOrderTests
{
    public class BasicTest
    {
        private const string ENTITY = "GarmentExternalPurchaseOrder";

        private const string USERNAME = "Unit Test";
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

        private GarmentExternalPurchaseOrderDataUtil dataUtil(GarmentExternalPurchaseOrderFacade facade, string testName)
        {
            var garmentPurchaseRequestFacade = new GarmentPurchaseRequestFacade(_dbContext(testName));
            var garmentPurchaseRequestDataUtil = new GarmentPurchaseRequestDataUtil(garmentPurchaseRequestFacade);

            var garmentInternalPurchaseOrderFacade = new GarmentInternalPurchaseOrderFacade(_dbContext(testName));
            var garmentInternalPurchaseOrderDataUtil = new GarmentInternalPurchaseOrderDataUtil(garmentInternalPurchaseOrderFacade, garmentPurchaseRequestDataUtil);

            return new GarmentExternalPurchaseOrderDataUtil(facade, garmentInternalPurchaseOrderDataUtil);
        }

        [Fact]
        public async void Should_Success_Create_Multiple_Data()
        {
            var facade = new GarmentExternalPurchaseOrderFacade(ServiceProvider,_dbContext(GetCurrentMethod()));
            var data = dataUtil(facade, GetCurrentMethod()).GetNewData();
            var Response = await facade.Create(data, USERNAME);
            Assert.NotEqual(Response, 0);
        }

        //[Fact]
        //public async void Should_Error_Create_Multiple_Data()
        //{
        //    var facade = new GarmentExternalPurchaseOrderFacade(_dbContext(GetCurrentMethod()));
        //    var data = dataUtil(facade, GetCurrentMethod()).GetNewData();
        //    foreach (var data in listData)
        //    {
        //        data.Items = null;
        //    }
        //    Exception e = await Assert.ThrowsAsync<Exception>(async () => await facade.CreateMultiple(listData, USERNAME));
        //    Assert.NotNull(e.Message);
        //}

        [Fact]
        public async void Should_Success_Get_All_Data()
        {
            var facade = new GarmentExternalPurchaseOrderFacade(ServiceProvider,_dbContext(GetCurrentMethod()));
            var data = dataUtil(facade, GetCurrentMethod()).GetNewData();
            var Response = facade.Read();
            Assert.NotEqual(Response.Item1.Count, 0);
        }

        [Fact]
        public async void Should_Success_Get_Data_By_Id()
        {
            var facade = new GarmentExternalPurchaseOrderFacade(ServiceProvider,_dbContext(GetCurrentMethod()));
            var data = dataUtil(facade, GetCurrentMethod()).GetNewData();
            var Response = facade.ReadById((int)data.Id);
            Assert.NotNull(Response);
        }

        //[Fact]
        //public void Should_Success_Validate_Data()
        //{
        //    var viewModel = new GarmentExternalPurchaseOrderViewModel
        //    {
        //        Items = null
        //    };
        //    Assert.True(viewModel.Validate(null).Count() > 0);
        //}
    }
}
