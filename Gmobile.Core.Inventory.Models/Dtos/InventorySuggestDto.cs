using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Dtos
{
    public class InventorySuggestDto
    {
        public long Id { get; set; }

        public string StockCode { get; set; }

        public string StockName { get; set; }

        public string StockType { get; set; }

        public int StockLevel { get; set; }
    }
}
