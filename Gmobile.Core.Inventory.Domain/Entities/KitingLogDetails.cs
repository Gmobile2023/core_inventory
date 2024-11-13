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
    [Alias("kiting_log_details")]
    public class KitingLogDetails : IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        public int KitingId { get; set; }
      
        [StringLength(15)] public string Mobile { get; set; }

        [StringLength(30)] public string Serial { get; set; }

        [StringLength(15)] public string Package { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
