using Gmobile.Core.Inventory.Models.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Dtos
{
    public class SettingDto
    {
        public string UserCreated { get; set; }
        public int StockId { get; set; }
        public SettingType Type { get; set; }
        public List<SettingItem> Items { get; set; }
    }

    public class SettingItem
    {
        public string Mobile { get; set; }

        public string Serial { get; set; }

        public string Package { get; set; }

        public decimal Price { get; set; }
    }

    public class MemberDto
    {
        public string Number { get; set; }

        public string Serial { get; set; }

        public decimal SalePrice { get; set; }
    }
}
