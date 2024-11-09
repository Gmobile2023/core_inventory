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
    [Alias("order_description")]
    public class OrderDescription : IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        public int OrderId { get; set; }

        [StringLength(500)] public string Description { get; set; }

        [StringLength(30)] public string ActionType { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
