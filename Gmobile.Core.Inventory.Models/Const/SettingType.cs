using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Const
{
    public enum SettingType : byte
    {
        Kiting = 1,//Ghép Kit
        UnKiting = 2,//Nhả kit
        SalePrice = 3,//Giá bán             
    }

    public enum PriceType : byte
    {
        Change = 1,//Thay đổi toàn bộ giá
        PlusRate = 2,//Công thêm tiền vào giá bán theo tỉ lệ %
        PlusExtra = 3//Cộng thêm tiền vào giá bán
    }

    public enum ObjectType : byte
    {
        All = 1,//Thay đổi toàn kho
        List = 2,//Thay đổi theo danh sách        
    }
}
