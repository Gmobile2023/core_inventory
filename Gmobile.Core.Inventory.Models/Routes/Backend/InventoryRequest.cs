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

    [Description("Kích hoạt kho")]
    [Tag(Name = "inventory")]
    [Route("/api/v1/inventory/stock/acive", "POST")]
    public class StockActiveRequest : IPost, IReturn<object>
    {
        public int Id { get; set; }
        public string UserActive { get; set; }
    }

    [Description("Thêm người bán hàng")]
    [Tag(Name = "inventory")]
    [Route("/api/v1/inventory/stock/sale", "POST")]
    public class StockAddSaleRequest : IPost, IReturn<object>
    {
        public int Id { get; set; }
        public string UserSale { get; set; }

        public string UserCreate { get; set; }
    }

    [Description("Chi tiết kho")]
    [Tag(Name = "inventory")]
    [Route("/api/v1/inventory/stock/{Id}", "GET")]
    public class StockDetailRequest : IGet, IReturn<object>
    {
        public int Id { get; set; }        
    }

    public class AccountRoleType
    {
        public string AccountCode { get; set; }

        public RoleType RoleType { get; set; }
    }


    [Description("Hàm lấy danh sách serial,số")]
    [Tag(Name = "inventory")]
    [Route("/api/v1/inventory/sim/list", "GET")]
    public class StockListSimRequest : IReturn<object>
    {
        public string StockCode { get; set; }     

        public int SimType { get; set; }

        public string Mobile { get; set; }

        public string Serial { get; set; }

        public string CategoryCode { get; set; }

        public string Attribute { get; set; }

        public int Status { get; set; }

        public int KitingStatus { get; set; }
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
    }

    [Description("Chi tiết số/serial")]
    [Tag(Name = "inventory")]
    [Route("/api/v1/inventory/sim/{Number}", "GET")]
    public class SimDetailRequest : IGet, IReturn<object>
    {
        public string Number { get; set; }

        public int SimType { get; set; }
    }
}
