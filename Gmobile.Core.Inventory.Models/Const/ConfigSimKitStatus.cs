using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Const
{
    public enum ConfigSimKitStatus: byte
    {
        Init = 0,//Khởi tạo
        Success = 1,//Đã ghép
        Lock = 3,//Khóa
        Cancel = 4//Hủy        
    }
}
