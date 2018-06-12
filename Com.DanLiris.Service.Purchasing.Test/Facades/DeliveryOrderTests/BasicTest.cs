using Com.DanLiris.Service.Purchasing.Lib.Models.DeliveryOrderModel;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.DanLiris.Service.Purchasing.Test.Facades.DeliveryOrderTests
{
    [Collection("ServiceProviderFixture Collection")]
    public class BasicTest
    {
        private IServiceProvider ServiceProvider { get; set; }

        public BasicTest(ServiceProviderFixture fixture)
        {
            ServiceProvider = fixture.ServiceProvider;

            IdentityService identityService = (IdentityService)ServiceProvider.GetService(typeof(IdentityService));
            identityService.Username = "Unit Test";
        }

        [Fact]
        public void Should_Success_Create_Data()
        {
            /*Coba Aja*/
            DeliveryOrder model = new DeliveryOrder();
            Assert.NotNull(model);
        }
    }
}
