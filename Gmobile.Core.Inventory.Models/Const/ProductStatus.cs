using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Const
{
    public enum ProductStatus : byte
    {
        Init = 0,
        Success = 1,
        Block = 2,
        Sale = 5,
        Fail = 3,
        Cancel = 4
    }
}
