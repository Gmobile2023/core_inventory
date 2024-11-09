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
    [Alias("product")]
    public class Product : IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        [StringLength(30)] public string StockCode { get; set; }

        [StringLength(30)] public string StockCurrentCode { get; set; }

        [StringLength(15)] public string Mobile { get; set; }

        [StringLength(15)] public string TelCo { get; set; }

        public ProductStatus Status { get; set; }

        [StringLength(50)] public string TransCode { get; set; }

        public decimal CostPrice { get; set; }

        public decimal SalePrice { get; set; }

        [StringLength(30)] public string UserCreated { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(30)] public string UserConfirm { get; set; }

        public DateTime? ConfirmDate { get; set; }

        [StringLength(30)] public string UserSale { get; set; }

        public DateTime? SaleDate { get; set; }

        [StringLength(150)] public string TreePath { get; set; }        
    }
}
