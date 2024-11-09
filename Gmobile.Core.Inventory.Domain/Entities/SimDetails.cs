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
    [Alias("sim_details")]
    public class SimDetails : AuditBase, IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        public int OrderDetailId { get; set; }
        [StringLength(30)] public string SimNumber { get; set; }
    }
}
