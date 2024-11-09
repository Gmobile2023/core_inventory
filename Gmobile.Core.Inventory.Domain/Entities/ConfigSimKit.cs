using ServiceStack.Model;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;
using Gmobile.Core.Inventory.Models.Const;

namespace Gmobile.Core.Inventory.Domain.Entities
{
    [Schema("public")]
    [Alias("config_sim_kit")]
    public class ConfigSimKit : IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        [StringLength(30)] public string StockCode { get; set; }

        [StringLength(15)] public string Mobile { get; set; }

        [StringLength(35)] public string Serial { get; set; }

        public ConfigSimKitStatus Status { get; set; }

        [StringLength(30)] public string UserCreated { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(30)] public string UserModify { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}
