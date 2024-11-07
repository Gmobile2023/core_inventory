namespace iZOTA.Core.Voucher.Models.Dtos;

public class RedemptionsResponse
{
    public double Amount { get; set; }
    public double TotalAmount { get; set; }
    public double DiscountAmount { get; set; }
    public string ReferenceCode { get; set; }
    public double? Cashback { get; set; }
    public string CurrencyCode { get; set; }
}