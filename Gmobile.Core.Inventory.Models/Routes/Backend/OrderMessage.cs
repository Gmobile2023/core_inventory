using Gmobile.Core.Inventory.Models.Const;
using Gmobile.Core.Inventory.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Routes.Backend
{
    public class OrderMessage
    {
        public string OrderCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }        
        public long OrderId { get; set; }
        public OrderSimType SimType { get; set; }
        public OrderTypeValue OrderType { get; set; }
        public OrderStatus Status { get; set; }
    }    
}
