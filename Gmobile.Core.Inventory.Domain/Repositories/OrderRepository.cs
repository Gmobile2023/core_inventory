using Gmobile.Core.Inventory.Domain.Entities;
using Gmobile.Core.Inventory.Models.Const;
using Gmobile.Core.Inventory.Models.Dtos;
using Gmobile.Core.Inventory.Models.Routes.Backend;
using Inventory.Shared.CacheManager;
using Inventory.Shared.Dtos.CommonDto;
using Microsoft.Extensions.Logging;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Domain.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IIventoryConnectionFactory _connectionFactory;
        private readonly ILogger<OrderRepository> _logger;
        public OrderRepository(IIventoryConnectionFactory connectionFactory, ILogger<OrderRepository> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách đơn hàng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<PagedResultDto<OrderDisplayDto>>> GetListOrder(OrderListRequest request)
        {
            try
            {
                using var data = await _connectionFactory.OpenAsync();
                var query = data.From<Entities.Order>().Where(p => true);

                if (!string.IsNullOrEmpty(request.StockCode))
                {
                    request.StockCode = request.StockCode.Trim();
                    query = query.Where(c => c.SrcStockCode.Contains(request.StockCode) || c.DesStockCode.Contains(request.StockCode));
                }

                if (!string.IsNullOrEmpty(request.OrderTitle))
                {
                    request.OrderTitle = request.OrderTitle.Trim();
                    query = query.Where(c => c.OrderTitle.Contains(request.OrderTitle));
                }

                if (request.OrderType > 0 && request.OrderType != 99)
                {
                    var orderType = (OrderTypeValue)request.OrderType;
                    query = query.Where(c => c.OrderType == orderType);
                }

                if (request.Status != 99)
                {
                    var status = (OrderStatus)request.Status;
                    query = query.Where(c => c.Status == status);
                }

                if (request.FromDate != null)
                {
                    query = query.Where(c => c.CreatedDate >= request.FromDate);
                }

                if (request.ToDate != null)
                {
                    query = query.Where(c => c.CreatedDate <= request.ToDate);
                }


                var queryTotal = query;
                query = query.OrderByDescending(c => c.CreatedDate)
                    .Skip(request.SkipCount).Take(request.MaxResultCount);

                var items = await data.SelectAsync(query);
                var total = await data.CountAsync(queryTotal);
                var dataView = items.ConvertTo<List<OrderDisplayDto>>();

                var reponse = new PagedResultDto<OrderDisplayDto>()
                {
                    Items = dataView,
                    TotalCount = total,
                };

                return ResponseMessageBase<PagedResultDto<OrderDisplayDto>>.Success(reponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetListOrder Exception : {ex}");
                return ResponseMessageBase<PagedResultDto<OrderDisplayDto>>.Success();
            }
        }

        /// <summary>
        /// Tạo đơn hàng
        /// </summary>
        /// <param name="orderDto"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<OrderMessage>> OrderCreate(OrderDto orderDto, List<OrderDetailDto> items)
        {
            using var data = await _connectionFactory.OpenAsync();
            using var trans = data.OpenTransaction();
            try
            {
                var order = orderDto.ConvertTo<Entities.Order>();
                order.CreatedBy = order.UserCreated;
                order.ModifiedBy = order.UserCreated;
                var orderDetails = items.ConvertTo<List<Entities.OrderDetails>>();
                var id = await data.InsertAsync(order, true);
                orderDetails.ForEach(c =>
                {
                    c.OrderId = (int)id;
                });

                await data.InsertAllAsync(orderDetails);
                trans.Commit();
                _logger.LogInformation($"OrderCreate {orderDto.ToJson()}. Success ");
                return ResponseMessageBase<OrderMessage>.Success(new OrderMessage()
                {
                    OrderCode = order.OrderCode,
                    Quantity = order.Quantity,
                    Amount = order.CostPrice,
                });
            }
            catch (Exception ex)
            {
                trans.Rollback();
                _logger.LogError($"Error OrderCreate {orderDto.ToJson()} Exception: {ex}");
                return ResponseMessageBase<OrderMessage>.Error("Tạo đơn hàng thất bại. Vui lòng kiểm tra lại thông tin !");
            }
        }

        /// <summary>
        /// Cập nhật đơn hàng
        /// </summary>
        /// <param name="orderDto"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<OrderMessage>> ConfirmOrder(OrderDto orderDto, OrderDescription orderDescription)
        {
            using var data = await _connectionFactory.OpenAsync();
            using var trans = data.OpenTransaction();
            try
            {
                var order = await data.SingleAsync<Entities.Order>(c => c.OrderCode == orderDto.OrderCode);
                order.UserConfirm = orderDto.UserConfirm;
                order.UserApprove = orderDto.UserApprove;
                order.ApproveDate = orderDto.ApproveDate;
                order.ConfirmDate = orderDto.ConfirmDate;
                order.ModifiedBy = orderDto.UserCreated;
                order.Status = orderDto.Status;
                orderDescription.OrderId = (int)orderDto.Id;
                orderDescription.CreatedDate = DateTime.Now;
                await data.UpdateAsync(order);
                await data.InsertAsync(orderDescription);
                trans.Commit();
                _logger.LogInformation($"ConfirmOrder {orderDto.OrderCode}. Success ");
                var messageItem = new OrderMessage()
                {
                    OrderCode = order.OrderCode,
                    Quantity = order.Quantity,
                    Amount = order.CostPrice,
                    OrderId = order.Id,
                    OrderType = order.OrderType,
                    SimType = order.SimType,
                    Status = order.Status,
                };
                return ResponseMessageBase<OrderMessage>.Success(messageItem);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                _logger.LogError($"Error ConfirmOrder {orderDto.ToJson()} Exception: {ex}");
                return ResponseMessageBase<OrderMessage>.Error("Cập nhật đơn hàng thất bại. Vui lòng kiểm tra lại thông tin !");
            }
        }


        /// <summary>
        /// Lấy chi tiết đơn hàng
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public async Task<OrderDto?> GetOrderByCode(string orderCode)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                var order = await data.SingleAsync<Entities.Order>(c => c.OrderCode == orderCode);
                var orderDto = order?.ConvertTo<OrderDto>();
                _logger.LogInformation($"GetOrderByCode OrderCode= {orderCode} . Success ");
                return orderDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error GetOrderByCode OrderCode= {orderCode}  Exception: {ex}");
                return null;
            }
        }

        public async Task<List<OrderDetailDto>> GetListOrderDetail(int orderId)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                var orderDetails = await data.SelectAsync<Entities.OrderDetails>(c => c.OrderId == orderId);
                var details = orderDetails.ConvertTo<List<OrderDetailDto>>();
                _logger.LogInformation($"GetListOrderDetail orderID= {orderId} . Success ");
                return details;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error GetListOrderDetail orderID= {orderId}  Exception: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Insert danh sách serial
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="serials"></param>
        /// <returns></returns>
        public async Task<int> SyncToSerial(string orderCode, List<Serials> serials)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                await data.InsertAllAsync(serials);
                _logger.LogInformation($"SyncToSerial OrderCode= {orderCode} - Total= {serials.Count} . Success ");
                return serials.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error SyncToSerial OrderCode= {orderCode} - Total= {serials.Count}  Exception: {ex}");
                return 0;
            }
        }

        /// <summary>
        /// Insert danh sách số 
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        public async Task<int> SyncToMobile(string orderCode, List<Product> products)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                await data.InsertAllAsync(products);
                _logger.LogInformation($"SyncToMobile OrderCode= {orderCode} - Total= {products.Count} . Success ");
                return products.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error SyncToMobile OrderCode= {orderCode} - Total= {products.Count}  Exception: {ex}");
                return 0;
            }
        }

        /// <summary>
        /// Lưu chi tiết thông tin sim/số
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="simDetails"></param>
        /// <returns></returns>
        public async Task<int> SyncSimDetails(string orderCode, List<SimDetails> simDetails)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                await data.InsertAllAsync(simDetails);
                _logger.LogInformation($"SyncSimDetails OrderCode= {orderCode} - Total= {simDetails.Count} . Success ");
                return simDetails.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error SyncSimDetails OrderCode= {orderCode} - Total= {simDetails.Count}  Exception: {ex}");
                return 0;
            }
        }

        public async Task<List<string>> GetListSerialToList(List<string> serials)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                var serialDetails = await data.SelectAsync<Entities.Serials>(c => serials.Contains(c.Serial));
                var details = serialDetails.Select(c => c.Serial).ToList();
                return details;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error GetListSerialToList Exception: {ex}");
                return null;
            }
        }

        public async Task<List<string>> GetListMobileToList(List<string> mobiles)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                var mobileDetails = await data.SelectAsync<Entities.Product>(c => mobiles.Contains(c.Mobile));
                var details = mobileDetails.Select(c => c.Mobile).ToList();
                return details;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error GetListMobileToList Exception: {ex}");
                return null;
            }
        }

        public async Task<bool> UpdateOrderTotalCurrent(OrderDto orderDto, List<OrderDetailDto> details)
        {
            using var data = await _connectionFactory.OpenAsync();
            using var trans = data.OpenTransaction();
            try
            {
                var orderDetails = await data.SelectAsync<Entities.OrderDetails>(c => c.OrderId == orderDto.Id);
                orderDetails.ForEach(c =>
                {
                    var d = details.FirstOrDefault(x => x.Id == c.Id);
                    if (d != null)
                        c.QuantityCurrent = d.QuantityCurrent;
                });
                var order = await data.SingleByIdAsync<Entities.Order>(orderDto.Id);
                order.QuantityCurrent = orderDto.QuantityCurrent;
                await data.UpdateAsync(order);
                await data.UpdateAllAsync(orderDetails);
                trans.Commit();
                _logger.LogInformation($"UpdateOrderTotalCurrent  OrderCode= {orderDto.OrderCode}. Success ");
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                _logger.LogError($"Error UpdateOrderTotalCurrent- OrderId= {orderDto.OrderCode} Exception: {ex}");
                return false;
            }
        }

        public async Task<List<SimDetails>> GetListSimDetailsByOrderDetailId(int orderDetailId)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                var details = await data.SelectAsync<Entities.SimDetails>(c => c.OrderDetailId == orderDetailId);
                return details;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error GetListSimDetailsByOrderDetailId Exception: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Cập nhật Số sang kho mới
        /// </summary>
        /// <param name="srcStockId"></param>
        /// <param name="desStock"></param>
        /// <param name="orderCode"></param>
        /// <param name="mobiles"></param>
        /// <returns></returns>
        public async Task<int> SyncExchangeStockToMobile(int srcStockId, InventoryDto desStock, string orderCode, List<string> mobiles)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                var mobileDetails = await data.SelectAsync<Entities.Product>(c => c.StockId == srcStockId
                && mobiles.Contains(c.Mobile) && c.Status == ProductStatus.Success);
                mobileDetails.ForEach(c =>
                {
                    c.StockId = (int)desStock.Id;
                    c.StockCurrentCode = desStock.StockCode;
                    c.TreePath = desStock.TreePath;
                    c.TransCode = orderCode;
                    c.ConfirmDate = DateTime.Now;
                });
                await data.UpdateAllAsync(mobileDetails);
                _logger.LogInformation($"SyncExchangeStockToMobile OrderCode= {orderCode} - Total= {mobileDetails.Count()} . Success ");
                return mobileDetails.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error SyncExchangeStockToMobile OrderCode= {orderCode} - Total= {mobiles.Count}  Exception: {ex}");
                return 0;
            }
        }

        /// <summary>
        /// Cập nhật Serial sang kho mới
        /// </summary>
        /// <param name="srcStockId"></param>
        /// <param name="desStock"></param>
        /// <param name="orderCode"></param>
        /// <param name="mobiles"></param>
        /// <returns></returns>
        public async Task<int> SyncExchangeStockToSerial(int srcStockId, InventoryDto desStock, string orderCode, List<string> mobiles)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                var serialDetails = await data.SelectAsync<Entities.Serials>(c => c.StockId == srcStockId
                && mobiles.Contains(c.Serial) && c.Status == SerialStatus.Success);
                serialDetails.ForEach(c =>
                {
                    c.StockId = (int)desStock.Id;
                    c.StockCurrentCode = desStock.StockCode;
                    c.TreePath = desStock.TreePath;
                    c.TransCode = orderCode;
                    c.ConfirmDate = DateTime.Now;
                });
                await data.UpdateAllAsync(serialDetails);
                _logger.LogInformation($"SyncExchangeStockToSerial OrderCode= {orderCode} - Total= {serialDetails.Count()} . Success ");
                return serialDetails.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error SyncExchangeStockToSerial OrderCode= {orderCode} - Total= {mobiles.Count}  Exception: {ex}");
                return 0;
            }
        }

    }
}
