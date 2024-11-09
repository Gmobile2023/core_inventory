using Gmobile.Core.Inventory.Models.Const;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Routes.Backend
{
    [Tag(Name = "inventory")]
    [Route("/api/v1/inventory/stocks", "GET")]
    public class StockListRequest : IReturn<object>
    {
        public string StockCode { get; set; }

        public string StockName { get; set; }

        public int CityId { get; set; }

        public int DistrictId { get; set; }

        public int WardId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int ParentId { get; set; }

        public int Status { get; set; }
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
    }

    [Description("Hàm tạo kho")]
    [Tag(Name = "inventory")]
    [Route("/api/v1/inventory/stock", "POST")]
    public class StockCreatedRequest : IPost, IReturn<object>
    {
        public string StockCode { get; set; }

        public string StockName { get; set; }

        public string StockType { get; set; }

        public int? CityId { get; set; }

        public string CityName { get; set; }

        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }

        public int? WardId { get; set; }

        public string WardName { get; set; }

        public int? ParentId { get; set; }

        public string Address { get; set; }

        public string UserCreated { get; set; }

        public List<AccountRoleType> RoleTypes { get; set; }
    }


    [Description("Hàm sửa kho")]
    [Tag(Name = "inventory")]
    [Route("/api/v1/inventory/stock", "PUT")]
    public class StockUpdateRequest : IPut, IReturn<object>
    {
        public int Id { get; set; }

        public string StockName { get; set; }

        public string StockType { get; set; }

        public int? CityId { get; set; }

        public string CityName { get; set; }

        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }

        public int? WardId { get; set; }

        public string WardName { get; set; }

        public int? ParentId { get; set; }

        public string Address { get; set; }

        public List<AccountRoleType> RoleTypes { get; set; }
    }

    public class AccountRoleType
    {
        public string AccountCode { get; set; }

        public RoleType RoleType { get; set; }
    }
}
