
using Gmobile.Core.Inventory.Domain.BusinessServices;
using Gmobile.Core.Inventory.Models.Routes.Backend;
using Inventory.Shared.Dtos.CommonDto;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System.Collections;
using System.IO;
using System.Text;

namespace Gmobile.Core.Inventory.Component.Services;

public class MainService : Service
{
    private readonly ILogger<MainService> _logger;
    private readonly IStockService _stockService;
    private readonly IOrderService _orderService;
    private readonly IFileService _fileService;


    public MainService(IStockService stockService, IOrderService orderService, IFileService fileService, ILogger<MainService> logger)
    {
        _stockService = stockService;
        _orderService = orderService;
        _fileService = fileService;
        _logger = logger;
    }

    #region 1.Phần kho
    public async Task<object> GetAsync(StockListRequest request)
    {
        _logger.LogInformation($"StockListRequest {request.ToJson()}");
        var rs = await _stockService.GetListInventory(request);
        return rs;
    }

    public async Task<object> GetAsync(StockSuggestsRequest request)
    {
        _logger.LogInformation($"StockSuggestsRequest {request.ToJson()}");
        var rs = await _stockService.GetSuggestsInventory(request);
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


    #endregion

    #region 2.Phần đơn hàng

    /// <summary>
    /// Danh sách đơn hàng
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<object> GetAsync(OrderListRequest request)
    {
        _logger.LogInformation($"OrderListRequest {request.ToJson()}");
        var rs = await _orderService.GetListOrder(request);
        return rs;
    }

    /// <summary>
    /// Tạo đơn hàng
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<object> PostAsync(OrderCreatedRequest request)
    {
        _logger.LogInformation($"OrderCreatedRequest {request.ToJson()}");
        var rs = await _orderService.OrderCreate(request);
        return rs;
    }

    /// <summary>
    /// Xác nhận đơn hàng
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<object> PutAsync(OrderConfirmRequest request)
    {
        _logger.LogInformation($"OrderConfirmRequest {request.ToJson()}");
        var rs = await _orderService.ConfirmOrder(request);
        return rs;
    }

    public async Task<object> PostAsync(StockKitingRequest request)
    {
        _logger.LogInformation($"StockKitingRequest {request.ToJson()}");

        try
        {
            //Xử lý thêm phần đọc file nữa  
            var file = Request.Files.FirstOrDefault();
            if (file != null)
            {
                var items = new List<Models.Dtos.SettingItem>();
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    var fileData = binaryReader.ReadBytes((int)file.ContentLength);
                    Stream stream = new MemoryStream(fileData);
                    items = await _fileService.ReadFileXls(stream);
                }

                var rs = await _stockService.KitingInventory(new Models.Dtos.SettingDto()
                {
                    StockId = request.StockId,
                    UserCreated = request.UserCreated,
                    Type = request.Type,
                    Items = items
                });
                return rs;
            }
            else
            {
                _logger.LogInformation($"StockKitingRequest Khong co file du lieu.");
                return ResponseMessageBase<string>.Error("Quý khách chưa upload file dữ liệu.");
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"StockKitingRequest Exception: {ex}");
            return ResponseMessageBase<string>.Error("Quý khách chưa upload file dữ liệu.");
        }
    }

    #endregion

}