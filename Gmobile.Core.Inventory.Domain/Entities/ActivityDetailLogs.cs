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
    [Alias("activity_detail_logs")]
    public class ActivityDetailLogs : IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        [StringLength(30)] public string Number { get; set; }

        [References(typeof(InventoryActivityLogs))]        
        public int LogId { get; set; }
    }
}
