using ServiceStack;

namespace Gmobile.Core.Inventory.Models.Routes.App;

[Tag(Name = "Order API")]
[Route("/api/v1/app/booking/order/{ProviderCode}/web-view-url", "GET")]
public class GetWebViewUrlRequest : IReturn<object>
{
    public string AccountCode { get; set; }
    public string ProviderCode { get; set; }
    public string ServiceCode { get; set; }
}