using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Lib.ViewModels.NewIntegrationViewModel
{
    public class SupplierViewModel
    {
            public string Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public bool Import { get; set; }
            public string Npwp { get; set; }
    }

}
