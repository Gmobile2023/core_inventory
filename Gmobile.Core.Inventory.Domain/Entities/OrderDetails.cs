using ServiceStack.Model;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace Gmobile.Core.Inventory.Domain.Entities
{
 
    public class OrderDetails : IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        public int OrderId { get; set; }

        public int Quantity { get; set; }

        [StringLength(50)] public string Ranger { get; set; }

        public decimal CostPrice { get; set; }

        public decimal SalePrice { get; set; }

        public int CategoryId { get; set; }

        [StringLength(30)] public string CategoryCode { get; set; }
    }
}
