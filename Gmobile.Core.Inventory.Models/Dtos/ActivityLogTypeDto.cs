using Gmobile.Core.Inventory.Models.Const;
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

        public string UserCreated { get; set; }

        public long KitingId { get; set; }
    }

    public class ActivityLogItemInit
    {
        public long LogId { get; set; }
        public string ActionType { get; set; }
        public string Description { get; set; }
        public string KeyCode { get; set; }
        public long KitingId { get; set; }
    }
}
