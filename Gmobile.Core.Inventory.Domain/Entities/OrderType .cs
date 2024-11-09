using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Domain.Entities
{
    [Schema("public")]
    [Alias("order_types")]
    public class OrderTypes : AuditBase, IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        [StringLength(30)] public string OrderType { get; set; }

        [StringLength(150)] public string OrderTypeName { get; set; }

        public int Status { get; set; }

        [StringLength(30)] public string UserCreated { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
