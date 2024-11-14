using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Dtos
{
    public class InventoryActivityLogDto
    {
        public long Id { get; set; }

        public string OrderCode { get; set; }

        public string ActionType { get; set; }

        public string Description { get; set; }

        public string UserCreated { get; set; }
        public DateTime CreatedDate { get; set; }

        public int? SrcStockId { get; set; }

        public int? DesStockId { get; set; }

        public string SrcStockCode { get; set; }

        public string DesStockCode { get; set; }

        public int Status { get; set; }
    }
}
