using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Const
{
    public enum OrderTypeValue : byte
    {
        Import = 1,//Nhập mới
        Transfer = 2,//Điều chuyển
        Recovery = 3,//Đơn thu hồi
        Sale = 5//Đơn bán hàng
    }

    public enum OrderSimType : byte
    {
        Mobile = 1,
        Serial = 2,
    }

    public enum OrderAttributeType : byte
    {
        Normal = 0,
        Diamond = 1,
        Yellow = 2,
        Silver = 3,
        Copper = 4,
        Default = 99,
    }
}
