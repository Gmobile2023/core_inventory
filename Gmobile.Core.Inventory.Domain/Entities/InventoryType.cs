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
    [Alias("inventory_type")]
    public class InventoryType : AuditBase, IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        [StringLength(20)] public string StockType { get; set; }

        [StringLength(50)] public string StockTypeName { get; set; }

        public int Status { get; set; }

        [StringLength(30)] public string UserCreated { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(30)] public string UserConfirm { get; set; }

        public DateTime ? ConfirmDate { get; set; }
    }
}
