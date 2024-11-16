using Gmobile.Core.Inventory.Models.Const;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Dtos
{
    public class InventoryRoleDto
    {
        public long Id { get; set; }

        public int StockId { get; set; }

        public string AccountCode { get; set; }

        public RoleType RoleType { get; set; }
    }
}
