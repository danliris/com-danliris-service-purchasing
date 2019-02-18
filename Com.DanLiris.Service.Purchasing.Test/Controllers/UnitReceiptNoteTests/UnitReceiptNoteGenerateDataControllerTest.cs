using Com.DanLiris.Service.Purchasing.Lib.Models.UnitReceiptNoteModel;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.UnitReceiptNoteViewModel;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.UnitReceiptNoteDataUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace Com.DanLiris.Service.Purchasing.Test.Controllers.ExternalPurchaseOrderTests
{
    [Collection("TestServerFixture Collection")]
    public class UnitReceiptNoteGenerateDataControllerTest
    {
        private const string MediaType = "application/json";
        private readonly string URI = "v1/generating-data/unit-receipt-note";

        private TestServerFixture TestFixture { get; set; }

        private HttpClient Client
        {
            get { return this.TestFixture.Client; }
        }

        protected UnitReceiptNoteDataUtil DataUtil
        {
            get { return (UnitReceiptNoteDataUtil)this.TestFixture.Service.GetService(typeof(UnitReceiptNoteDataUtil)); }
        }

        public UnitReceiptNoteGenerateDataControllerTest(TestServerFixture fixture)
        {
            TestFixture = fixture;
        }

        [Fact]
        public async Task Should_Success_Get_Report_Excel()
        {
            var response = await this.Client.GetAsync(URI + "/download");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
