using Gmobile.Core.Inventory.Models.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Dtos
{
    public class KitingDto
    {
        public string UserCreated { get; set; }
        public int StockId { get; set; }
        public KitingType Type { get; set; }
        public List<KitingItem> Items { get; set; }
    }

    public class KitingItem
    {
        public string Mobile { get; set; }

        public string Serial { get; set; }

        public string Package { get; set; }
    }
}
