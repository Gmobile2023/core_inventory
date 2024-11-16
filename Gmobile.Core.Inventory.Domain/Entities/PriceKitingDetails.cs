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
    [Alias("price_kiting_details")]
    public class PriceKitingDetails : IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        public int SettingId { get; set; }
      
        [StringLength(15)] public string Mobile { get; set; }

        [StringLength(30)] public string Serial { get; set; }

        [StringLength(15)] public string Package { get; set; }

        public decimal Price { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
