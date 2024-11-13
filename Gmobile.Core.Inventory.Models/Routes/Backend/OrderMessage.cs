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
    }    
}
