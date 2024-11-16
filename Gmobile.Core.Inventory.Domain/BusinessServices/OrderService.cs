using Elasticsearch.Net;
using Gmobile.Core.Inventory.Domain.Entities;
using Gmobile.Core.Inventory.Domain.Repositories;
using Gmobile.Core.Inventory.Models.Const;
using Gmobile.Core.Inventory.Models.Dtos;
using Gmobile.Core.Inventory.Models.Routes.Backend;
using Inventory.Shared.Const;
using Inventory.Shared.Dtos.CommonDto;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grpc.Core.ChannelOption;

namespace Gmobile.Core.Inventory.Domain.BusinessServices
{
    public class OrderService : IOrderService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;
        public OrderService(IStockRepository stockRepository, IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _stockRepository = stockRepository;
            _orderRepository = orderRepository;
            // _transCodeGenerator = transCodeGenerator;
            _logger = logger;
        }
        public async Task<ResponseMessageBase<PagedResultDto<OrderDisplayDto>>> GetListOrder(OrderListRequest request)
        {
            return await _orderRepository.GetListOrder(request);
        }

        /// <summary>
        /// Xử lý phần đơn hàng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<OrderMessage>> OrderCreate(OrderCreatedRequest request)
        {
            #region 1.Validate

            request.StockCode = (request.StockCode ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(request.StockCode))
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách chưa truyền mã kho");
            }

            request.CategoryCode = (request.CategoryCode ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(request.CategoryCode))
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách chưa truyền mã loại sản phẩm");
            }

            request.UserCreated = (request.UserCreated ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(request.UserCreated))
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách chưa người thực hiện");
            }

            if (request.Items == null || request.Items.Count <= 0)
            {
                return ResponseMessageBase<OrderMessage>.Error("Chi tiết đơn hàng đang để trống.");
            }

            var categoryDto = await _stockRepository.GetCategoryDetail(request.CategoryCode);

            if (categoryDto == null)
            {
                return ResponseMessageBase<OrderMessage>.Error("Không tồn tại loại sản phẩm.");
            }

            //Xử lý check kỹ thông tin đơn hàng
            var items = new List<OrderDetailDto>();
            foreach (var item in request.Items)
            {
                var checkRs = GetTotalRangeData(item.FromRange, item.ToRange);
                if (checkRs.ResponseStatus.ErrorCode != ResponseCodeConst.Success)
                    return ResponseMessageBase<OrderMessage>.Error(checkRs.ResponseStatus.ErrorCode, checkRs.ResponseStatus.Message);

                items.Add(new OrderDetailDto()
                {
                    CategoryId = (int)categoryDto.Id,
                    CategoryCode = categoryDto.CategoryCode,
                    CostPrice = 0,
                    Quantity = Convert.ToInt32(checkRs.Results),
                    Range = $"{item.FromRange}-{item.ToRange}",
                    Attribute = item.Attribute,
                    TelCo = item.TelCo ?? string.Empty,
                });
            }

            var inventoryDto = await _stockRepository.GetInventoryDetail(request.StockCode);
            if (inventoryDto == null)
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách kiểm tra lại thông tin kho.");
            }

            #endregion

            #region 2.Thực thi đơn hàng

            var orderDto = new OrderDto()
            {
                DesStockCode = inventoryDto.StockCode,
                DesStockId = (int)inventoryDto.Id,
                OrderType = OrderTypeValue.Import,
                OrderTitle = request.Title,
                Description = request.Description,
                UserCreated = request.UserCreated,
                CreatedDate = DateTime.Now,
                Quantity = items.Sum(c => c.Quantity),
                CostPrice = items.Sum(c => c.CostPrice),
                SimType = request.SimType,
                Status = OrderStatus.Init,
            };

            orderDto.OrderCode = $"PN{DateTime.Now.ToString("yyMMddHHmmssfff")}";//await _transCodeGenerator.TransCodeGeneratorAsync("PN");
            var messagerOrder = await _orderRepository.OrderCreate(orderDto, items);
            if (messagerOrder.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
                await _stockRepository.ActivitysLog(new ActivityLogTypeDto()
                {
                    ActionType = request.SimType == OrderSimType.Serial
                    ? ActivityLogTypeValue.CreateSerial
                    : ActivityLogTypeValue.CreateMobile,
                    StockLevel = inventoryDto.StockType,
                    DesStockName = inventoryDto.StockName,
                    DesStockCode = inventoryDto.StockCode,
                    UserCreated = request.UserCreated,
                    OrderCode = orderDto.OrderCode,
                });
            #endregion

            return messagerOrder;
        }

        /// <summary>
        /// Xác nhận đơn hàng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<OrderMessage>> ConfirmOrder(OrderConfirmRequest request)
        {
            #region 1.Validate

            if (string.IsNullOrEmpty(request.OrderCode))
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách chưa truyền mã đơn hàng");
            }

            request.UserCreated = (request.UserCreated ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(request.UserCreated))
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách chưa người thực hiện");
            }

            //Xử lý check kỹ thông tin đơn hàng           
            var orderDto = await _orderRepository.GetOrderByCode(request.OrderCode);
            if (orderDto == null)
            {
                return ResponseMessageBase<OrderMessage>.Error("Không tìm thấy thông tin đơn hàng.");
            }

            if (orderDto.Status is OrderStatus.Cancel or OrderStatus.Fail)
            {
                return ResponseMessageBase<OrderMessage>.Error("Đơn hàng đã bị hủy.");
            }

            if (orderDto.Status is OrderStatus.Approve && request.Status is OrderStatus.Approve)
            {
                return ResponseMessageBase<OrderMessage>.Error("Đơn hàng đã được duyệt.");
            }

            if (orderDto.Status is OrderStatus.Confirm && request.Status is OrderStatus.Confirm)
            {
                return ResponseMessageBase<OrderMessage>.Error("Đơn hàng đã được xác nhận.");
            }

            #endregion

            #region 2.Thực thi đơn hàng

            var status = request.Status;           
            if (status == OrderStatus.Approve)
            {
                orderDto.ApproveDate = DateTime.Now;
                orderDto.UserApprove = request.UserCreated;
            }
            else if (status == OrderStatus.Confirm)
            {
                orderDto.ConfirmDate = DateTime.Now;
                orderDto.UserConfirm = request.UserCreated;
            }

            orderDto.Status = status;
            var actionTypeDto = await _stockRepository.GetActionTypeByOrderInfo(orderDto.OrderType, orderDto.SimType, orderDto.Status);
            var messagerOrder = await _orderRepository.ConfirmOrder(orderDto, new Entities.OrderDescription()
            {
                ActionType = actionTypeDto!=null? actionTypeDto.AcitonType:string.Empty,
                Description = request.Description,
            });

            if (messagerOrder.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
            {
                string stockName = string.Empty;
                if (orderDto.Status == OrderStatus.Confirm)
                {
                    var stockDto = await _stockRepository.GetInventoryDetail(orderDto.DesStockCode);
                    stockName = stockDto != null ? stockDto.StockName : orderDto.DesStockCode;
                    var orderDetails = await _orderRepository.GetListOrderDetail((int)orderDto.Id);
                    await ScanToData(stockDto, orderDto, orderDetails);
                }
                await _stockRepository.ActivitysLog(new ActivityLogTypeDto()
                {
                    ActionType = actionTypeDto != null ? actionTypeDto.AcitonType : string.Empty,
                    DesStockName = orderDto.DesStockCode,
                    DesStockCode = stockName,
                    UserCreated = request.UserCreated,
                    OrderCode = orderDto.OrderCode,
                });
            }
            #endregion

            return messagerOrder;
        }

        /// <summary>
        /// Lấy danh sách Serial/số
        /// </summary>
        /// <param name="fromRange"></param>
        /// <param name="toRange"></param>
        /// <returns></returns>
        private ResponseMessageBase<List<string>> GenRangeData(string fromRange, string toRange)
        {
            if (fromRange.Length != toRange.Length)
                return ResponseMessageBase<List<string>>.Error("Dải serial không hợp lệ, độ dài không bằng nhau.");

            try
            {
                int index = 0;
                for (int i = 0; i < fromRange.Length; i++)
                {
                    if (fromRange[i] != toRange[i])
                    {
                        index = i;
                        break;
                    }
                }

                string souce = fromRange.Substring(0, index);
                var rFrom = fromRange.Substring(index, fromRange.Length - index);
                var rTo = toRange.Substring(index, toRange.Length - index);

                var p = rTo.Length;
                var sTo = Convert.ToDouble(rTo);
                var sFrom = Convert.ToDouble(rFrom);
                if (sFrom > sTo)
                    return ResponseMessageBase<List<string>>.Error("Dải sô không hợp lệ, dải từ lớn hơn dải tới.");

                double temp = sFrom;
                var lit = new List<string>();
                while (temp <= sTo)
                {
                    lit.Add($"{souce}{temp.ToString().PadLeft(p, '0')}");
                    temp = temp + 1;
                }

                _logger.LogInformation($"GenRangeData: [{fromRange}->{toRange}] => {lit.Count} Done.");
                return ResponseMessageBase<List<string>>.Success(lit);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GenRangeData: [{fromRange}->{toRange}] Exception : {ex}");
                return ResponseMessageBase<List<string>>.Error("Lấy danh sách không thành công.");
            }
        }

        private ResponseMessageBase<double> GetTotalRangeData(string fromRange, string toRange)
        {
            if (fromRange.Length != toRange.Length)
                return ResponseMessageBase<double>.Error("Dải serial không hợp lệ, độ dài không bằng nhau.");

            try
            {
                int index = 0;
                for (int i = 0; i < fromRange.Length; i++)
                {
                    if (fromRange[i] != toRange[i])
                    {
                        index = i;
                        break;
                    }
                }

                var rFrom = fromRange.Substring(index, fromRange.Length - index);
                var rTo = toRange.Substring(index, toRange.Length - index);

                var sTo = Convert.ToDouble(rTo);
                var sFrom = Convert.ToDouble(rFrom);

                if (sFrom > sTo)
                    return ResponseMessageBase<double>.Error("Dải số không hợp lệ, dải từ lớn hơn dải tới.");

                var quantity = sTo - sFrom + 1;
                _logger.LogInformation($"GetTotalRangeData: [{fromRange}->{toRange}] => {quantity} Done.");
                return ResponseMessageBase<double>.Success(quantity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetTotalRangeData: [{fromRange}->{toRange}] Exception : {ex}");
                return ResponseMessageBase<double>.Error("Không kiểm tra được dải số.");
            }
        }

        private async Task<bool> ScanToData(InventoryDto? stockDto, OrderDto orderDto, List<OrderDetailDto> details)
        {
            foreach (var x in details)
            {
                //1.Nhap moi về kho
                if (orderDto.OrderType == OrderTypeValue.Import)
                {
                    if (orderDto.SimType == OrderSimType.Serial)
                        x.QuantityCurrent = await AddToSerial(stockDto, orderDto, x);
                    else x.QuantityCurrent = await AddToMobile(stockDto, orderDto, x);
                }
                else if (orderDto.OrderType == OrderTypeValue.Transfer)
                {
                    x.QuantityCurrent = await ExchangeStockToData(stockDto ?? new InventoryDto(), orderDto, x);
                }
            }
            orderDto.QuantityCurrent = details.Sum(c => c.QuantityCurrent);
            await _orderRepository.UpdateOrderTotalCurrent(orderDto, details);
            return true;
        }

        /// <summary>
        /// Duyệt Serial vào kho
        /// </summary>
        /// <param name="orderDto"></param>
        /// <param name="details"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        private async Task<int> AddToSerial(InventoryDto? stockDto, OrderDto orderDto, OrderDetailDto details)
        {

            int totalCurrent = 0;
            try
            {
                var ranger = details.Range.Split('-');
                var reponse = GenRangeData(ranger[0], ranger[1]);
                var dataRanger = reponse.Results;
                var lt = dataRanger.Take(ConstTakeCount.TakeCount).ToList();
                var serialTmp = lt.Select(c => c).ToList();
                dataRanger = dataRanger.Except(serialTmp).ToList();
                int scanInt = 0;
                while (lt.Count > 0)
                {
                    _logger.LogInformation($"AddToSerial_Step: {scanInt} - OrderCode= {orderDto.OrderCode} - simType= {orderDto.SimType} - run_row = {lt.Count}");
                    var serialTmps = lt.Select(c => c).ToList();
                    var searchs = await _orderRepository.GetListSerialToList(serialTmps);
                    if (searchs != null)
                    {
                        if (searchs.Count > 0)
                            lt = lt.Except(searchs.ToList()).ToList();

                        _logger.LogInformation($"AddToSerial_Step: {scanInt} - OrderCode= {orderDto.OrderCode} - simType= {orderDto.SimType} - run_curent = {lt.Count}");
                        var arrays = (from x in lt
                                      select new Serials
                                      {
                                          CategorId = details.CategoryId,
                                          CategoryCode = details.CategoryCode,
                                          CostPrice = details.CostPrice,
                                          Serial = x,
                                          Status = SerialStatus.Success,
                                          CreatedDate = orderDto.CreatedDate,
                                          StockCode = orderDto.DesStockCode,
                                          StockId = orderDto.DesStockId ?? 0,
                                          SalePrice = details.SalePrice,
                                          StockCurrentCode = orderDto.DesStockCode,
                                          SouceTransCode = orderDto.OrderCode,
                                          UserCreated = orderDto.UserCreated,
                                          TreePath = stockDto != null ? stockDto.TreePath : string.Empty,
                                          TransCode = string.Empty,
                                      }).ToList();

                        totalCurrent = totalCurrent + await _orderRepository.SyncToSerial(orderDto.OrderCode, arrays);
                    }

                    lt = dataRanger.Take(ConstTakeCount.TakeCount).ToList();
                    serialTmp = lt.Select(c => c).ToList();
                    dataRanger = dataRanger.Except(serialTmp).ToList();
                    scanInt = scanInt + 1;
                }

                return totalCurrent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AddToData => OrderCode= {orderDto.OrderCode} Exception: {ex}");
                return totalCurrent;
            }
        }

        /// <summary>
        /// Duyệt số vào kho
        /// </summary>
        /// <param name="orderDto"></param>
        /// <param name="details"></param>
        /// <param name="taskCount"></param>
        /// <returns></returns>
        private async Task<int> AddToMobile(InventoryDto? stockDto, OrderDto orderDto, OrderDetailDto details)
        {
            int totalCurrent = 0;
            try
            {
                var ranger = details.Range.Split('-');
                var reponse = GenRangeData(ranger[0], ranger[1]);
                var dataRanger = reponse.Results;
                var lt = dataRanger.Take(ConstTakeCount.TakeCount).ToList();
                var serialTmp = lt.Select(c => c).ToList();
                dataRanger = dataRanger.Except(serialTmp).ToList();
                int scanInt = 0;
                while (lt.Count > 0)
                {
                    _logger.LogInformation($"AddToMobile_Step: {scanInt} - OrderCode= {orderDto.OrderCode} - simType= {orderDto.SimType} - run_row = {lt.Count}");
                    var serialTmps = lt.Select(c => c).ToList();
                    var searchs = await _orderRepository.GetListMobileToList(serialTmps);
                    if (searchs != null)
                    {
                        if (searchs.Count > 0)
                            lt = lt.Except(searchs.ToList()).ToList();

                        _logger.LogInformation($"AddToMobile_Step: {scanInt} - OrderCode= {orderDto.OrderCode} - simType= {orderDto.SimType} - run_curent = {lt.Count}");
                        var arrays = (from x in lt
                                      select new Product
                                      {
                                          CategoryId = details.CategoryId,
                                          CategoryCode = details.CategoryCode,
                                          CostPrice = details.CostPrice,
                                          Mobile = x,
                                          Status = ProductStatus.Success,
                                          Attribute = details.Attribute,
                                          CreatedDate = orderDto.CreatedDate,
                                          StockCode = orderDto.DesStockCode,
                                          StockId = orderDto.DesStockId ?? 0,
                                          SalePrice = details.SalePrice,
                                          StockCurrentCode = orderDto.DesStockCode,
                                          UserCreated = orderDto.UserCreated,
                                          TelCo = details.TelCo,
                                          SouceTransCode = orderDto.OrderCode,
                                          TreePath = stockDto != null ? stockDto.TreePath : string.Empty,
                                          Serial = string.Empty,
                                          KitingStatus = 0,
                                          TransCode = string.Empty,
                                      }).ToList();

                        totalCurrent = totalCurrent + await _orderRepository.SyncToMobile(orderDto.OrderCode, arrays);
                    }

                    lt = dataRanger.Take(ConstTakeCount.TakeCount).ToList();
                    serialTmp = lt.Select(c => c).ToList();
                    dataRanger = dataRanger.Except(serialTmp).ToList();
                    scanInt = scanInt + 1;
                }

                return totalCurrent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AddToData => OrderCode= {orderDto.OrderCode} Exception: {ex}");
                return totalCurrent;
            }
        }


        /// <summary>
        /// Tạo đơn hàng điều chuyển giữa 2 kho
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<OrderMessage>> OrderTransferCreate(OrderTransferCreatedRequest request)
        {
            #region *****.Validate

            request.SrcStockCode = (request.SrcStockCode ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(request.SrcStockCode))
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách chưa truyền kho chuyển");
            }

            request.DesStockCode = (request.DesStockCode ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(request.DesStockCode))
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách chưa truyền kho nhận");
            }

            request.CategoryCode = (request.CategoryCode ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(request.CategoryCode))
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách chưa truyền mã loại sản phẩm");
            }

            request.UserCreated = (request.UserCreated ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(request.UserCreated))
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách chưa người thực hiện");
            }


            if (request.ObjectType == ObjectType.List)
            {
                if (request.RangeItems == null || request.RangeItems.Count <= 0)
                    return ResponseMessageBase<OrderMessage>.Error("Quý khách chưa truyền danh sách sim/số để tạo phiếu.");
            }
            else
            {
                if (request.RangeRule == null || request.RangeRule.Quantity <= 0)
                    return ResponseMessageBase<OrderMessage>.Error("Quý khách kiểm tra lại số lượng cần tạo yêu cầu.");
            }


            var categoryDto = await _stockRepository.GetCategoryDetail(request.CategoryCode);

            if (categoryDto == null)
            {
                return ResponseMessageBase<OrderMessage>.Error("Không tồn tại loại sản phẩm.");
            }

            //Xử lý check kỹ thông tin đơn hàng

            var inventorySrcDto = await _stockRepository.GetInventoryDetail(request.SrcStockCode);
            if (inventorySrcDto == null)
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách kiểm tra lại thông tin kho chuyển.");
            }

            var inventoryDesDto = await _stockRepository.GetInventoryDetail(request.DesStockCode);
            if (inventoryDesDto == null)
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách kiểm tra lại thông tin kho nhận.");
            }

            var list = await _stockRepository.GetListDataTransfer((int)inventorySrcDto.Id, request.SimType, request.ObjectType, request.RangeRule, request.RangeItems);
            if (list == null && list?.Count() <= 0)
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách kiểm tra lại thông tin sim/số.");
            }

            #endregion

            #region 2.Thực thi đơn hàng

            var items = new List<OrderDetailDto>();
            items.Add(new OrderDetailDto()
            {
                CategoryCode = categoryDto.CategoryCode,
                CategoryId = (int)categoryDto.Id,
                CostPrice = 0,
                Quantity = list?.Count() ?? 0,
                TelCo = string.Empty,
                Range = string.Empty,
                SalePrice = list?.Sum(c => c.SalePrice) ?? 0,
            });
            var orderDto = new OrderDto()
            {
                SrcStockCode = inventorySrcDto.StockCode,
                SrcStockId = (int)inventorySrcDto.Id,
                DesStockCode = inventoryDesDto.StockCode,
                DesStockId = (int)inventoryDesDto.Id,
                OrderType = OrderTypeValue.Transfer,
                OrderTitle = request.Title,
                Description = request.Description,
                UserCreated = request.UserCreated,
                CreatedDate = DateTime.Now,
                Quantity = items.Sum(c => c.Quantity),
                CostPrice = items.Sum(c => c.CostPrice),
                SimType = request.SimType,
                Status = OrderStatus.Init,
            };

            var lstData = list?.Select(c => c.Number).ToList() ?? new List<string>();
            orderDto.OrderCode = $"PX{DateTime.Now.ToString("yyMMddHHmmssfff")}";//await _transCodeGenerator.TransCodeGeneratorAsync("PN");
            var messagerOrder = await _orderRepository.OrderCreate(orderDto, items);
            if (messagerOrder.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
            {
                orderDto = await _orderRepository.GetOrderByCode(orderDto.OrderCode);
                await AddTranferToDataSimDetails(orderDto, lstData);
                await _stockRepository.ActivitysLog(new ActivityLogTypeDto()
                {
                    ActionType = request.SimType == OrderSimType.Serial
                    ? ActivityLogTypeValue.ApproveTransferSerial
                    : ActivityLogTypeValue.ApproveTransferMobile,
                    StockLevel = inventorySrcDto.StockType,
                    StockId = (int)inventorySrcDto.Id,
                    DesStockCode = inventoryDesDto.StockCode,
                    DesStockName = inventoryDesDto.StockName,
                    SrcStockCode = inventorySrcDto.StockCode,
                    SrcStockName = inventorySrcDto.StockName,
                    UserCreated = request.UserCreated,
                    OrderCode = orderDto.OrderCode,
                });
            }
            #endregion

            return messagerOrder;
        }


        /// <summary>
        /// Đồng bộ sim/số chi tiết
        /// </summary>
        /// <param name="orderDto"></param>
        /// <param name="orderDetailId"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        private async Task<int> AddTranferToDataSimDetails(OrderDto orderDto, List<string> details)
        {
            int totalCurrent = 0;
            try
            {
                var orderDetails = await _orderRepository.GetListOrderDetail((int)orderDto.Id);
                var orderDetailId = (int)orderDetails.First().Id;
                var dataRanger = details;
                var lt = dataRanger.Take(ConstTakeCount.TakeCount).ToList();
                var serialTmp = lt.Select(c => c).ToList();
                dataRanger = dataRanger.Except(serialTmp).ToList();
                int scanInt = 0;
                while (lt.Count > 0)
                {
                    _logger.LogInformation($"AddTranferToDataSimDetails_Step: {scanInt} - OrderCode= {orderDto.OrderCode} - simType= {orderDto.SimType} - run_row = {lt.Count}");
                    var serialTmps = lt.Select(c => c).ToList();
                    var searchs = await _orderRepository.GetListMobileToList(serialTmps);
                    if (searchs != null)
                    {
                        _logger.LogInformation($"AddTranferToDataSimDetails_Step: {scanInt} - OrderCode= {orderDto.OrderCode} - simType= {orderDto.SimType} - run_curent = {searchs.Count}");
                        var arrays = (from x in searchs
                                      select new SimDetails
                                      {
                                          OrderDetailId = orderDetailId,
                                          SimNumber = x,
                                      }).ToList();

                        totalCurrent = totalCurrent + await _orderRepository.SyncSimDetails(orderDto.OrderCode, arrays);
                    }

                    lt = dataRanger.Take(ConstTakeCount.TakeCount).ToList();
                    serialTmp = lt.Select(c => c).ToList();
                    dataRanger = dataRanger.Except(serialTmp).ToList();
                    scanInt = scanInt + 1;
                }

                return totalCurrent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AddTranferToDataSimDetails => OrderCode= {orderDto.OrderCode} Exception: {ex}");
                return totalCurrent;
            }
        }

        /// <summary>
        /// Xuất điều chuyển kho
        /// </summary>
        /// <param name="desStockDto"></param>
        /// <param name="orderDto"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        private async Task<int> ExchangeStockToData(InventoryDto desStockDto, OrderDto orderDto, OrderDetailDto details)
        {
            int totalCurrent = 0;
            try
            {
                var simDetails = await _orderRepository.GetListSimDetailsByOrderDetailId((int)details.Id);
                var ranges = simDetails.Select(c => c.SimNumber).ToList();
                var lt = ranges.Take(ConstTakeCount.TakeCount).ToList();
                var sTmp = lt.Select(c => c).ToList();
                ranges = ranges.Except(sTmp).ToList();
                int scanInt = 0;
                while (lt.Count > 0)
                {
                    _logger.LogInformation($"ExchangeStockToData_Step: {scanInt} - OrderCode= {orderDto.OrderCode} - SimType= {orderDto.SimType} - run_row = {lt.Count}");
                    var currnetNow = orderDto.SimType == OrderSimType.Mobile
                        ? await _orderRepository.SyncExchangeStockToMobile(orderDto.SrcStockId ?? 0, desStockDto, orderDto.OrderCode, lt)
                        : await _orderRepository.SyncExchangeStockToSerial(orderDto.SrcStockId ?? 0, desStockDto, orderDto.OrderCode, lt);

                    totalCurrent = totalCurrent + currnetNow;
                    lt = ranges.Take(ConstTakeCount.TakeCount).ToList();
                    sTmp = lt.Select(c => c).ToList();
                    ranges = ranges.Except(sTmp).ToList();
                    scanInt = scanInt + 1;
                }

                return totalCurrent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ExchangeStockToData => OrderCode= {orderDto.OrderCode} - SimType= {orderDto.SimType} Exception: {ex}");
                return totalCurrent;
            }
        }
    }
}
