using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Const
{  
    public enum CategoryStatus : byte
    {
        Init = 0,
        Success = 1,
        Cancel = 2,
        Fail = 3
    }
}
