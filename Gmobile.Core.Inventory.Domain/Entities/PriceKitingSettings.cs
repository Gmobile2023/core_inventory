using Gmobile.Core.Inventory.Models.Const;
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
    [Alias("price_kiting_settings")]
    public class PriceKitingSettings : IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        public int StockId { get; set; }

        public int Status { get; set; }

        public SettingType Type { get; set; }

        public int Quantity { get; set; }

        public int QuantityCurrent { get; set; }

        [StringLength(30)] public string UserCreated { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
