namespace Inventory.Shared.Const;

public class ResponseCodeConst
{
    public const string Success = "00"; //Thành công
    public const string Error = "01"; //Lỗi
    public const string InvalidAuth = "02";
    public const string InvalidSignature = "03";
    public const string PartnerNotFound = "04";
    public const string PartnerNotActive = "05";
    public const string ServiceNotActive = "06";
    public const string AccountNotAllowService = "07";
    public const string InvalidSecretKey = "08";
    public const string RequestReceived = "09"; //Đã tiếp nhận giao dịch
    public const string RequestAlreadyExists = "10"; //Giao dịch đối tác đã tồn tại
    public const string Cancel = "11";
    public const string TransactionNotFound = "12"; //Giao dịch không tồn tại
    public const string TimeOut = "13";
    public const string InProcessing = "14";
    public const string WaitForResult = "15";
    public const string Paid = "16";
    public const string CardNotInventory = "17"; //KHo thẻ k đủ

    public const string PhoneLocked = "19"; //Số điện thoại đã bị khóa
    public const string PhoneNotValid = "20"; //Số điện thoại k hợp lệ
    public const string NotEzpay = "21"; //Chưa có TK ezpay
    public const string PhoneLockTopup = "22"; //Khóa chiều nạp
    public const string NotValidStatus = "23"; //Giao dịch không thành công. Vui lòng kiểm tra thông tin của thuê bao
    public const string ErrorProvider = "24"; //Lỗi từ phía nhà cc

    public const string InvalidPerpaid = "26"; //K phải thuê bao trả trước
    public const string ServiceConfigNotValid = "27"; //KHông lấy được cấu hình kênh
    public const string ProductNotFound = "28"; //Sản phẩm không được hỗ trợ
    public const string ProductValueNotValid = "29"; //Mệnh giá nạp không hợp lệ

    public const string
        TransactionError = "30"; //Mã lỗi quy định các gd đã xác định là lỗi luôn. k qua các kênh khác nữa

    public const string BalanceNotEnough = "31";
    
    
    public const string BillException = "32"; //K thể truy vấn thông tin hóa đơn
    public const string BillException33 = "33"; //Hóa đơn đã được thanh toán hoặc chưa phát sinh nợ cước
    public const string BillException34 = "34"; //Hóa đơn đã được thanh toán hoặc chưa phát sinh nợ cước
    public const string BillException35 = "35"; //Hóa đơn đã được thanh toán hoặc chưa phát sinh nợ cước
    public const string BillException36 = "36"; //Hóa đơn đã được thanh toán hoặc chưa phát sinh nợ cước
    public const string BillException37 = "37"; //Hóa đơn đã được thanh toán hoặc chưa phát sinh nợ cước
    public const string BillException38 = "38"; //Hóa đơn đã được thanh toán hoặc chưa phát sinh nợ cước
    public const string BillException39 = "39"; //Hóa đơn đã được thanh toán hoặc chưa phát sinh nợ cước
    public const string BillException40 = "40"; //Hóa đơn đã được thanh toán hoặc chưa phát sinh nợ cước
                                                
    public const string BankNotExist = "41";//Chưa có tài khoản ngân hàng
    public const string BankExist = "42";//Đã có tài khoản ngân hàng
    public const string BankNotLinkAccount = "43";//Chưa liên kết ngân hàng
    public const string BankLinkAccount = "44";//Tài khoản đã liên kết ngân hàng
    public const string BankNoActive = "45";//Tài khoản chưa kích hoạt
    public const string AccountNoEkyc = "46";//Tài khoản chưa Ekyc
    public const string NoEkyc = "48";//Chưa ekyc tài khoản
    public const string MaxIdCard = "49";//Vượt quá 3 căn cước
    public const string IssuedTicket = "50";// Thông tin Đặt chỗ đã được Xuất vé.";
    public const string ConfirmPaymentGw = "1000";
}