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
    [Alias("inventory_settings")]
    public class InventorySettings : IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        public DateTime TimeStart { get; set; }

        public DateTime ? TimeStop { get; set; }

        public int Status { get; set; }

        [StringLength(200)] public string Description { get; set; }
    }
}
