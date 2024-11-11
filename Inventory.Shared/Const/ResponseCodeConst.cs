namespace Inventory.Shared.Const;

public class ResponseCodeConst
{
    public const string Success = "00"; //Thành công
    public const string Error = "01"; //Lỗi
    public const string InvalidAuth = "02";
    public const string InvalidSignature = "03";
    public const string WaitForResult = "15";
    public const string ExistCode = "10";
    public const string EmptyName = "11";
    public const string EmptyLevel = "12";
    public const string ExportRecover = "13";
}