using Gmobile.Core.Inventory.Models.Const;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Dtos
{
    public class SimNumberDto
    {
        public string Serial { get; set; }
        public string Mobile { get; set; }


        public long Id { get; set; }

        public string StockCode { get; set; }

        public string StockCurrentCode { get; set; }

        public string TelCo { get; set; }

        public ProductStatus Status { get; set; }

        public int KitingStatus { get; set; }

        public string TransCode { get; set; }

        public decimal CostPrice { get; set; }

        public decimal SalePrice { get; set; }

        public string UserCreated { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UserConfirm { get; set; }

        public DateTime? ConfirmDate { get; set; }

        public string UserSale { get; set; }

        public DateTime? SaleDate { get; set; }

        public string TreePath { get; set; }
    }
}
