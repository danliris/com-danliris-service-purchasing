using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.AutoMapperProfiles;
using Com.DanLiris.Service.Purchasing.Lib.Helpers.ReadResponse;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentSupplierBalanceDebtModel;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentSupplierBalanceDebtViewModel;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.NewIntegrationViewModel;
using Com.DanLiris.Service.Purchasing.Test.Helpers;
using Com.DanLiris.Service.Purchasing.WebApi.Controllers.v1.GarmentSupplierBalanceDebtControllers;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.DanLiris.Service.Purchasing.Test.Controllers.GarmentSupplierBalanceDebtControllerTests
{
    public class GarmentSupplierBalanceDebtControllerTest
    {
        private GarmentSupplierBalanceDebtViewModel viewModel
        {
            get
            {
                return new GarmentSupplierBalanceDebtViewModel
                {
                    supplier = new SupplierViewModel { Name = "", Import = false, },
                    codeRequirment = "",
                    currency = new CurrencyViewModel { Code = "", Rate = 0, Symbol = "", Description = "", },
                    totalValas = 0,
                    totalAmountIDR = 0,
                    Year = 0,
                };
            }
        }
        private GarmentSupplierBalanceDebt Model
        {
            get
            {
                return new GarmentSupplierBalanceDebt { };
            }
        }
        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            return serviceProvider;
        }
        private ServiceValidationExeption GetServiceValidationExeption()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            List<ValidationResult> validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(this.viewModel, serviceProvider.Object, null);
            return new ServiceValidationExeption(validationContext, validationResults);
        }
        private GarmentSupplierBalanceDebtController GetController(Mock<IBalanceDebtFacade> facadeM, Mock<IValidateService> validateM, Mock<IMapper> mapper)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            var servicePMock = GetServiceProvider();

            if (validateM != null)
            {
                servicePMock
                    .Setup(x => x.GetService(typeof(IValidateService)))
                    .Returns(validateM.Object);
            }

            var controller = new GarmentSupplierBalanceDebtController(facadeM.Object, mapper.Object, servicePMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = user.Object
                    }
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";

            return controller;
        }
        protected int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        [Fact]
        public void Should_Success_Get_All_Data()
        {
            var mockFacade = new Mock<IBalanceDebtFacade>();

            mockFacade.Setup(x => x.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, It.IsAny<string>()))
                .Returns(Tuple.Create(new List<GarmentSupplierBalanceDebt>(), 0, new Dictionary<string, string>()));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<List<GarmentSupplierBalanceDebtViewModel>>(It.IsAny<List<GarmentSupplierBalanceDebt>>()))
                .Returns(new List<GarmentSupplierBalanceDebtViewModel> { viewModel });

            GarmentSupplierBalanceDebtController controller = GetController(mockFacade,null, mockMapper);
            var response = controller.Get();
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }
        [Fact]
        public void Should_Error_Get_All_Data()
        {
            var mockFacade = new Mock<IBalanceDebtFacade>();

            mockFacade.Setup(x => x.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, It.IsAny<string>()))
                .Returns(Tuple.Create(new List<GarmentSupplierBalanceDebt>(), 0, new Dictionary<string, string>()));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<List<GarmentSupplierBalanceDebtViewModel>>(It.IsAny<List<GarmentSupplierBalanceDebt>>()))
                .Returns(new List<GarmentSupplierBalanceDebtViewModel> { viewModel });

            GarmentSupplierBalanceDebtController controller = new GarmentSupplierBalanceDebtController(mockFacade.Object, mockMapper.Object, GetServiceProvider().Object);
            var response = controller.Get();
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
        [Fact]
        public async Task Should_Success_Create_Data()
        {
            var validateMock = new Mock<IValidateService>();
            validateMock.Setup(s => s.Validate(It.IsAny<GarmentSupplierBalanceDebtViewModel>())).Verifiable();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<GarmentSupplierBalanceDebt>(It.IsAny<GarmentSupplierBalanceDebtViewModel>()))
                .Returns(Model);

            var mockFacade = new Mock<IBalanceDebtFacade>();
            mockFacade.Setup(x => x.Create(It.IsAny<GarmentSupplierBalanceDebt>(), "unittestusername", 7))
               .ReturnsAsync(1);

            var controller = GetController(mockFacade, validateMock, mockMapper);

            var response = await controller.Post(this.viewModel);
            Assert.Equal((int)HttpStatusCode.Created, GetStatusCode(response));
        }
        [Fact]
        public async Task Should_Error_Create_Data()
        {
            var validateMock = new Mock<IValidateService>();
            validateMock.Setup(s => s.Validate(It.IsAny<GarmentSupplierBalanceDebtViewModel>())).Verifiable();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<GarmentSupplierBalanceDebt>(It.IsAny<GarmentSupplierBalanceDebtViewModel>()))
                .Returns(Model);

            var mockFacade = new Mock<IBalanceDebtFacade>();
            mockFacade.Setup(x => x.Create(It.IsAny<GarmentSupplierBalanceDebt>(), "unittestusername", 7))
               .ReturnsAsync(1);

            var controller = new GarmentSupplierBalanceDebtController(mockFacade.Object, mockMapper.Object, GetServiceProvider().Object); ;

            var response = await controller.Post(this.viewModel);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
        [Fact]
        public void Should_Success_Get_Loader()
        {
            var mockFacade = new Mock<IBalanceDebtFacade>();

            mockFacade.Setup(x => x.ReadLoader(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), null, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<dynamic>(new List<dynamic>(), 1, new Dictionary<string, string>()));

            var mockMapper = new Mock<IMapper>();

            GarmentSupplierBalanceDebtController controller = GetController(mockFacade, null, mockMapper);
            var response = controller.GetLoader();
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }
        [Fact]
        public void Should_Error_Get_Loader()
        {
            var mockFacade = new Mock<IBalanceDebtFacade>();

            mockFacade.Setup(x => x.ReadLoader(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), null, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            var mockMapper = new Mock<IMapper>();

            GarmentSupplierBalanceDebtController controller = GetController(mockFacade, null, mockMapper);
            var response = controller.GetLoader();
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
        [Fact]
        public void Should_Success_Get_Data_By_Id()
        {
            var mockFacade = new Mock<IBalanceDebtFacade>();

            mockFacade.Setup(x => x.ReadById(It.IsAny<int>()))
                .Returns(new GarmentSupplierBalanceDebt());

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<GarmentSupplierBalanceDebtViewModel>(It.IsAny<GarmentSupplierBalanceDebt>()))
                .Returns(viewModel);

            GarmentSupplierBalanceDebtController controller = new GarmentSupplierBalanceDebtController(mockFacade.Object, mockMapper.Object, GetServiceProvider().Object);
            var response = controller.Get(It.IsAny<int>());
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void Should_Error_Get_Data_By_Id()
        {
            var mockFacade = new Mock<IBalanceDebtFacade>();

            mockFacade.Setup(x => x.ReadById(It.IsAny<int>()))
                .Returns(new GarmentSupplierBalanceDebt());

            var mockMapper = new Mock<IMapper>();

            GarmentSupplierBalanceDebtController controller = new GarmentSupplierBalanceDebtController(mockFacade.Object, mockMapper.Object, GetServiceProvider().Object);
            var response = controller.Get(It.IsAny<int>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
        [Fact]
        public async Task Should_Validate_Create_Data()
        {
            var validateMock = new Mock<IValidateService>();
            validateMock.Setup(s => s.Validate(It.IsAny<GarmentSupplierBalanceDebtViewModel>())).Throws(GetServiceValidationExeption());

            var mockMapper = new Mock<IMapper>();

            var mockFacade = new Mock<IBalanceDebtFacade>();

            var controller = GetController(mockFacade, validateMock, mockMapper);

            var response = await controller.Post(this.viewModel);
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }


        //[Fact]
        //public void UploadFile_WithoutException_ReturnOK()
        //{
        //    string header = "Kode Supplier, Supplier, Import, Bulan, Tahun, Valas, Mata Uang, Kurs, Nilai(IDR), Jenis Bahan";
        //    string isi = "KodeSupplier, Supplier, false,1, 2010,12, Mata Uang, 1, 1, Jenis";
        //    var mockFacade = new Mock<IBalanceDebtFacade>();
        //    mockFacade.Setup(f => f.UploadData(It.IsAny<List<GarmentSupplierBalanceDebt>>(), "unittestusername")).ReturnsAsync(1);
        //    mockFacade.Setup(f => f.CsvHeader).Returns(header.Split(',').ToList());

        //    mockFacade.Setup(f => f.UploadValidate(ref It.Ref<List<GarmentSupplierBalanceDebtViewModel>>.IsAny, It.IsAny<List<KeyValuePair<string, StringValues>>>())).Returns(new Tuple<bool, List<object>>(true, new List<object>()));
        //    GarmentSuppliersBalanceDebtProfile profile = new GarmentSuppliersBalanceDebtProfile();

        //    var mockMapper = new Mock<IMapper>();
        //    mockMapper.Setup(x => x.ConfigurationProvider).Returns(new MapperConfiguration(cfg => cfg.AddProfile(profile)));
        //    var model = new GarmentSupplierBalanceDebt()
        //    {
        //        DOCurrencyCode = "CobaCode",
        //        DOCurrencyRate = 1,
        //        CodeRequirment = "CobaRequir",
        //        CreatedBy = "UnitTest",
        //        Import = false,
        //        SupplierName = "SupplierTest123",
        //        TotalValas = 123,
        //        TotalAmountIDR = 1235435
        //    };
        //    mockMapper.Setup(x => x.Map<List<GarmentSupplierBalanceDebt>>(It.IsAny<List<GarmentSupplierBalanceDebtViewModel>>())).Returns(new List<GarmentSupplierBalanceDebt>() { model });
        //    //var mockIdentityService = new Mock<IdentityService>();
        //    var mockValidateService = new Mock<IValidateService>();

        //    var controller = GetController( mockFacade, mockMapper);
        //    controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = $"{It.IsAny<int>()}";
        //    controller.ControllerContext.HttpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
        //    var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(header + "\n" + isi)), 0, Encoding.UTF8.GetBytes(header + "\n" + isi).LongLength, "Data", "test.csv");
        //    controller.ControllerContext.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });

        //    var response = controller.PostCSVFilec();
        //    Assert.NotNull(response);
        //}

        //[Fact]
        //public void UploadFile_WithException_ReturnError()
        //{
        //    var mockFacade = new Mock<IBalanceDebtFacade>();
        //    mockFacade.Setup(f => f.UploadData(It.IsAny<List<GarmentSupplierBalanceDebt>>(), "unittestusername")).Throws(new Exception());

        //    var mockMapper = new Mock<IMapper>();

        //   // var mockIdentityService = new Mock<IIdentityService>();

        //    //var mockValidateService = new Mock<IValidateService>();

        //    var controller = GetController(mockFacade, mockMapper);
        //    controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = $"{It.IsAny<int>()}";

        //    var response = controller.PostCSVFilec();
        //    Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        //}
        //[Fact]
        //public void UploadFile_WithException_FileNotFound()
        //{
        //    string header = "Kode Supplier, Supplier, Import, Bulan, Tahun, Valas, Mata Uang, Kurs, Nilai(IDR), Jenis Bahan";
        //    var mockFacade = new Mock<IBalanceDebtFacade>();
        //    //var mockFacade = new Mock<IBalanceDebtFacade>();
        //    mockFacade.Setup(f => f.UploadData(It.IsAny<List<GarmentSupplierBalanceDebt>>(), "unittestusername")).ReturnsAsync(1);
        //    mockFacade.Setup(f => f.CsvHeader).Returns(header.Split(',').ToList());

        //    mockFacade.Setup(f => f.UploadValidate(ref It.Ref<List<GarmentSupplierBalanceDebtViewModel>>.IsAny, It.IsAny<List<KeyValuePair<string, StringValues>>>())).Returns(new Tuple<bool, List<object>>(true, new List<object>()));
        //    GarmentSuppliersBalanceDebtProfile profile = new GarmentSuppliersBalanceDebtProfile();
        //    GarmentSupplierBalanceDebt Model = new GarmentSupplierBalanceDebt();

        //    var mockMapper = new Mock<IMapper>();
        //    mockMapper.Setup(x => x.ConfigurationProvider).Returns(new MapperConfiguration(cfg => cfg.AddProfile(profile)));

        //    mockMapper.Setup(x => x.Map<List<GarmentSupplierBalanceDebt>>(It.IsAny<List<GarmentSupplierBalanceDebtViewModel>>())).Returns(new List<GarmentSupplierBalanceDebt>() { Model });
        //    //var mockIdentityService = new Mock<IIdentityService>();
        //   // var mockValidateService = new Mock<IValidateService>();

        //    var controller = GetController(mockFacade, mockMapper);
        //    controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = $"{It.IsAny<int>()}";
        //    controller.ControllerContext.HttpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
        //    var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(header + "\n" + header)), 0, Encoding.UTF8.GetBytes(header + "\n" + header).LongLength, "Data", "test.csv");
        //    controller.ControllerContext.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { });

        //    var response = controller.PostCSVFilec();
        //    Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        //}
        //[Fact]
        //public void UploadFile_WithException_CSVError()
        //{
        //    string header = "Kode Supplier, Supplier, Import, Bulan, Tahun, Valas, Mata Uang, Kurs, Nilai(IDR), Jenis Bahan";
        //    var mockFacade = new Mock<IBalanceDebtFacade>();
        //    mockFacade.Setup(f => f.UploadData(It.IsAny<List<GarmentSupplierBalanceDebt>>(), "unittestusername")).Verifiable();
        //    mockFacade.Setup(f => f.CsvHeader).Returns(header.Split(';').ToList());
        //    var data = It.IsAny<List<GarmentSupplierBalanceDebtViewModel>>();
        //    mockFacade.Setup(f => f.UploadValidate(ref It.Ref<List<GarmentSupplierBalanceDebtViewModel>>.IsAny, It.IsAny<List<KeyValuePair<string, StringValues>>>())).Returns(new Tuple<bool, List<object>>(false, new List<object>()));
        //    GarmentSuppliersBalanceDebtProfile profile = new GarmentSuppliersBalanceDebtProfile();
        //    GarmentSupplierBalanceDebt Model = new GarmentSupplierBalanceDebt();
        //    var mockMapper = new Mock<IMapper>();
        //    mockMapper.Setup(x => x.ConfigurationProvider).Returns(new MapperConfiguration(cfg => cfg.AddProfile(profile)));

        //    mockMapper.Setup(x => x.Map<List<GarmentSupplierBalanceDebt>>(It.IsAny<List<GarmentSupplierBalanceDebtViewModel>>())).Returns(new List<GarmentSupplierBalanceDebt>() { Model });

        //    var controller = GetController(mockFacade, mockMapper);
        //    controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = $"{It.IsAny<int>()}";
        //    controller.ControllerContext.HttpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
        //    var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(header + "\n" + header)), 0, Encoding.UTF8.GetBytes(header + "\n" + header).LongLength, "Data", "test.csv");
        //    controller.ControllerContext.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });

        //    var response = controller.PostCSVFilec();
        //    Assert.Equal((int)HttpStatusCode.NotFound, GetStatusCode(response));
        //}

        //[Fact]
        //public void UploadFile_WithException_ErrorInFile()
        //{
        //    string header = "Kode Supplier, Supplier, Import, Bulan, Tahun, Valas, Mata Uang, Kurs, Nilai(IDR), Jenis Bahan";
        //    string isi = "KodeSupplier, Supplier, false,1, 2010,12, Mata Uang, 1, 1, Jenis";
        //    var mockFacade = new Mock<IBalanceDebtFacade>();
        //    mockFacade.Setup(f => f.UploadData(It.IsAny<List<GarmentSupplierBalanceDebt>>(),"unittestusername")).Verifiable();
        //    mockFacade.Setup(f => f.CsvHeader).Returns(header.Split(',').ToList());
        //    var data = It.IsAny<List<GarmentSupplierBalanceDebtViewModel>>();
        //    mockFacade.Setup(f => f.UploadValidate(ref It.Ref<List<GarmentSupplierBalanceDebtViewModel>>.IsAny, It.IsAny<List<KeyValuePair<string, StringValues>>>())).Returns(new Tuple<bool, List<object>>(false, new List<object>()));
        //    GarmentSuppliersBalanceDebtProfile profile = new GarmentSuppliersBalanceDebtProfile();
        //    GarmentSupplierBalanceDebt Model = new GarmentSupplierBalanceDebt();
        //    var mockMapper = new Mock<IMapper>();
        //    mockMapper.Setup(x => x.ConfigurationProvider).Returns(new MapperConfiguration(cfg => cfg.AddProfile(profile)));

        //    mockMapper.Setup(x => x.Map<List<GarmentSupplierBalanceDebt>>(It.IsAny<List<GarmentSupplierBalanceDebtViewModel>>())).Returns(new List<GarmentSupplierBalanceDebt>() { Model });


        //    var controller = GetController(mockFacade, mockMapper);
        //    controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = $"{It.IsAny<int>()}";
        //    controller.ControllerContext.HttpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
        //    var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(header + "\n" + isi)), 0, Encoding.UTF8.GetBytes(header + "\n" + isi).LongLength, "Data", "test.csv");
        //    controller.ControllerContext.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });

        //    var response = controller.PostCSVFilec();
        //    Assert.NotNull(response);
        //}


    }
}
