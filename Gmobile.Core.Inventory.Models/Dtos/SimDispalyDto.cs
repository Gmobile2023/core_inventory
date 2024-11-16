using Gmobile.Core.Inventory.Models.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Dtos
{
    public class SimDispalyDto
    {
        public long Id { get; set; }
        public string Serial { get; set; }
        public string Mobile { get; set; }     

        public string StockCode { get; set; }

        public string StockCurrentCode { get; set; }

        public string TelCo { get; set; }

        public ProductStatus Status { get; set; }

        public int KitingStatus { get; set; }      

        public decimal CostPrice { get; set; }

        public decimal SalePrice { get; set; }  

        public DateTime CreatedDate { get; set; }      
    }
}
