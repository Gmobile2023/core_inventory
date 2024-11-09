
using Gmobile.Core.Inventory.Domain.BusinessServices;
using Gmobile.Core.Inventory.Models.Routes.Backend;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace Gmobile.Core.Inventory.Component.Services;

public class MainService : Service
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

    public async Task<object> PostAsync(StockActiveRequest request)
    {
        _logger.LogInformation($"StockActiveRequest {request.ToJson()}");
        var rs = await _stockService.ActiveInventory(request);
        _logger.LogInformation($"StockActive Reponse: {rs.ToJson()}");
        return rs;
    }

    public async Task<object> PostAsync(StockAddSaleRequest request)
    {
        _logger.LogInformation($"StockAddSaleRequest {request.ToJson()}");
        var rs = await _stockService.AddSaleToInventory(request);
        _logger.LogInformation($"StockAddSale Reponse: {rs.ToJson()}");
        return rs;
    }


    public async Task<object> GetAsync(StockDetailRequest request)
    {
        _logger.LogInformation($"StockDetailRequest {request.ToJson()}");
        var rs = await _stockService.GetDetailInventory(request.Id);
        _logger.LogInformation($"StockDetail Reponse: {rs.ToJson()}");
        return rs;
    }

    public async Task<object> GetAsync(StockListSimRequest request)
    {
        _logger.LogInformation($"StockListSimRequest {request.ToJson()}");
        var rs = await _stockService.GetListSimInventory(request);
        _logger.LogInformation($"StockListSim Reponse: {rs.ToJson()}");
        return rs;
    }

    public async Task<object> GetAsync(SimDetailRequest request)
    {
        _logger.LogInformation($"SimDetailRequest {request.ToJson()}");
        var rs = await _stockService.GetSimDetailInventory(request.Number, request.SimType);
        _logger.LogInformation($"SimDetail Reponse: {rs.ToJson()}");
        return rs;
    }

}