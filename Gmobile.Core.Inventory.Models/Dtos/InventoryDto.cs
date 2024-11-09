using Gmobile.Core.Inventory.Models.Const;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Dtos
{
    public class InventoryDto
    {
        public long Id { get; set; }

        public string StockCode { get; set; }

        public string StockName { get; set; }

        public string StockType { get; set; }

        public InventoryStatus Status { get; set; }

        public bool IsActive { get; set; }

        public int? ParentStockId { get; set; }

        public string TreePath { get; set; }

        public int? CityId { get; set; }

        public string CityName { get; set; }

        public int? DistrictId { get; set; }

        public string DistrictName { get; set; }

        public int? WardId { get; set; }

        public string WardName { get; set; }
        public string Address { get; set; }

        public string UserCreated { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UserConfirm { get; set; }

        public DateTime? ConfirmDate { get; set; }
    }
}
