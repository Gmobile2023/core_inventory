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
    [Alias("inventory_activity_logs")]
    public class InventoryActivityLogs : IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        [StringLength(50)] public string OrderCode { get; set; }

        [StringLength(30)] public string ActionType { get; set; }

        [StringLength(200)] public string Content { get; set; }

        [StringLength(30)] public string UserCreated { get; set; }
        public DateTime CreatedDate { get; set; }


        [References(typeof(Inventory))]
        public int? SrcStockId { get; set; }

        [References(typeof(Inventory))]
        public int? DesStockId { get; set; }

        [StringLength(25)] public string SrcStockCode { get; set; }

        [StringLength(25)] public string DesStockCode { get; set; }     
        public int Status { get; set; }
    }
}
