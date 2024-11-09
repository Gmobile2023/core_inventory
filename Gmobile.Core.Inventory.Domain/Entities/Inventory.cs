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
    [Alias("inventory")]
    public class Inventory : IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        [StringLength(30)] public string StockCode { get; set; }

        [StringLength(150)] public string StockName { get; set; }

        [StringLength(30)] public string StockType { get; set; }

        public InventoryStatus Status { get; set; }

        public bool IsActive { get; set; }

        public int? ParentStockId { get; set; }

        [StringLength(150)] public string TreePath { get; set; }

        public int? CityId { get; set; }

        [StringLength(50)] public string CityName { get; set; }

        public int? DistrictId { get; set; }

        [StringLength(50)] public string DistrictName { get; set; }

        public int? WardId { get; set; }

        [StringLength(50)] public string WardName { get; set; }
        public string Address { get; set; }

        [StringLength(30)] public string UserCreated { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(30)] public string UserConfirm { get; set; }

        public DateTime? ConfirmDate { get; set; }

    }
}
