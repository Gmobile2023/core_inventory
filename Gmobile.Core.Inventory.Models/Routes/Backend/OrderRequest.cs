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
    [Description("Danh sách đơn hàng")]
    [Tag(Name = "Order")]
    [Route("/api/v1/Stock/Orders", "GET")]
    public class OrderListRequest : IGet, IReturn<object>
    {
        public int OrderType { get; set; }

        public string OrderCode { get; set; }

        public string OrderTitle { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string StockCode { get; set; }

        public int Status { get; set; }
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
    }


    [Description("Tạo đơn hàng")]
    [Tag(Name = "Order")]
    [Route("/api/v1/Stock/Order", "POST")]
    public class OrderCreatedRequest : IPost, IReturn<object>
    {      
        public string CategoryCode { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string StockCode { get; set; }

        public string UserCreated { get; set; }

        public OrderSimType SimType { get; set; }

        public List<OrderItem> Items { get; set; }

    }


    [Description("Xác nhận đơn hàng")]
    [Tag(Name = "Order")]
    [Route("/api/v1/Stock/Order", "PUT")]
    public class OrderConfirmRequest : IPut, IReturn<object>
    {
        public string OrderCode { get; set; }       

        public string Description { get; set; }       

        public string UserCreated { get; set; }

        /// <summary>
        /// 2.Duyệt đơn
        /// 3.Xác nhân đơn
        /// </summary>
        public OrderStatus Status { get; set; }

    }

    public class OrderItem
    {        
        public string OrderName { get; set; }

        public string Unit { get; set; }

        public OrderAttributeType Attribute { get; set; }

        public string TelCo { get; set; }

        public string FromRange { get; set; }

        public string ToRange { get; set; }

        public int Quantity { get; set; }
    }


    [Description("Tạo đơn điều chuyển số/sim 2 kho")]
    [Tag(Name = "Transfer")]
    [Route("/api/v1/Stock/Transfer", "POST")]
    public class OrderTransferCreatedRequest : IPost, IReturn<object>
    {
        public string CategoryCode { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Mã kho chuyển
        /// </summary>
        public string SrcStockCode { get; set; }

        /// <summary>
        /// Mã kho nhận
        /// </summary>
        public string DesStockCode { get; set; }

        public string UserCreated { get; set; }

        public ObjectType ObjectType { get; set; }

        public OrderSimType SimType { get; set; }

        /// <summary>
        /// Lấy sim/số theo điều kiện lọc
        /// </summary>
        public OrderItem RangeRule { get; set; }
        /// <summary>
        /// Lấy sim/số theo hướng lựa chọn
        /// </summary>
        public List<string> RangeItems { get; set; }
        
    }

}
