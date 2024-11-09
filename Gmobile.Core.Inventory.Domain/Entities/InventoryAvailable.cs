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
    [Schema("public")]
    [Alias("inventory_available")]
    public class InventoryAvailable : AuditBase, IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        [StringLength(25)] public string StockCode { get; set; }

        [StringLength(25)] public string CategoryCode { get; set; }

        public int Quantity { get; set; }

        public int Limit { get; set; }

        public int MinLimit { get; set; }

        [StringLength(50)] public string LastTransCode { get; set; }

        public DateTime? LastTransDate { get; set; }

    }
}
