using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Dtos
{
    public class ActivityLogTypeDto
    {
        public string OrderCode { get; set; }

        public string ActionType { get; set; }

        public string DesStockCode { get; set; }

        public string DesStockName { get; set; }

        public string SrcStockCode { get; set; }

        public string SrcStockName { get; set; }

        public string StockLevel { get; set; }

        public string UserProcess { get; set; }
    }
}
