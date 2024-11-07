
using Microsoft.Extensions.Logging;

namespace Gmobile.Core.Inventory.Component.Services;

public class MainService : AppServiceBase
{
    private readonly ILogger<MainService> _logger;

    public MainService(ILogger<MainService> logger)
    {
        _logger = logger;
    }

    // public async Task<ResponseMessageBase<string>> PutAsync(BookingConfirmOrderRequest request)
    // {
    //     _logger.LogInformation($"BookingConfirmOrderRequest {request.ToJson()}");
    //     var rs = await _orderProcess.IpnRequest(request.ProviderCode, request.OrderCode, new ResStatus(ResponseCodeConst.Success));
    //     _logger.LogInformation($"BookingConfirmOrderRequest return {rs.ToJson()}");
    //     return new ResponseMessageBase<string>
    //     {
    //         ResponseStatus = rs.ResponseStatus
    //     };
    // }
}