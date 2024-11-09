
using Gmobile.Core.Inventory.Domain.BusinessServices;
using Gmobile.Core.Inventory.Models.Routes.Backend;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace Gmobile.Core.Inventory.Component.Services;

public class MainService : AppServiceBase
{
    private readonly ILogger<MainService> _logger;
    private readonly IStockService _stockService;


    public MainService(IStockService stockService, ILogger<MainService> logger)
    {
        _stockService = stockService;
        _logger = logger;
    }

    public async Task<object> GetAsync(StockListRequest request)
    {
        _logger.LogInformation($"StockListRequest {request.ToJson()}");
        var rs = await _stockService.GetListInventory(request);
        return rs;
    }

    public async Task<object> PostAsync(StockCreatedRequest request)
    {
        _logger.LogInformation($"StockCreatedRequest {request.ToJson()}");
        var rs = await _stockService.CreateInventory(request);
        _logger.LogInformation($"StockCreated Reponse: {rs.ToJson()}");
        return rs;
    }

    public async Task<object> PutAsync(StockUpdateRequest request)
    {
        _logger.LogInformation($"StockUpdateRequest {request.ToJson()}");
        var rs = await _stockService.UpdateInventory(request);
        _logger.LogInformation($"StockUpdate Reponse: {rs.ToJson()}");
        return rs;
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