using Gmobile.Core.Inventory.Models.Const;
using ProtoBuf.Meta;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Dtos
{
    public class OrderDto
    {
        public long Id { get; set; }
        public string OrderCode { get; set; }

        public OrderValueType OrderType { get; set; }

        public string OrderTitle { get; set; }

        /// <summary>
        /// 1: Đơn cho số
        /// 2: Đơn cho sim
        /// </summary>
        public OrderSimType SimType { get; set; }

        [StringLength(30)] public string SrcStockCode { get; set; }

        [StringLength(30)] public string DesStockCode { get; set; }

        public int? DesStockId { get; set; }

        public int? SrcStockId { get; set; }

        public string TransRef { get; set; }

        public string SaleTransCode { get; set; }
        public string ProviderCode { get; set; }
        public string UserCreated { get; set; }

        public string UserConfirm { get; set; }

        public string UserApprove { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public DateTime? ApproveDate { get; set; }        
        public decimal Fee { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public int Quantity { get; set; }
        public int QuantityCurrent { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public OrderStatus Status { get; set; }       
        public string Description { get; set; }
        public string Document { get; set; }
    }
}
