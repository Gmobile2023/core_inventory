using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Const
{
    public enum RoleType : byte
    {
        UserManager = 0,
        UserCreate = 1,
        UserApprove = 2,
        UserAccounting = 4,
        UserSale = 5,
    }
}
