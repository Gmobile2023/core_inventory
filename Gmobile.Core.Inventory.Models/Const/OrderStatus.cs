namespace Gmobile.Core.Inventory.Models.Const;

public enum OrderStatus : byte
{
    Init = 0,// Khởi tạo-> Chờ duyệt
    Success = 1,//Hoàn tất/ thành công
    Approve = 2,//Đã duyệt -> Chờ xác nhận
    Confirm = 5,//Đã xác nhận -> Chờ hoàn tất
    Fail = 3,//Lỗi
    Cancel = 4//Đã hủy
}