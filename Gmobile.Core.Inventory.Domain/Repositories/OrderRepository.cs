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
                    query = query.Where(c => c.OrderType.Contains(request.OrderTitle));
                }

                if (!string.IsNullOrEmpty(request.OrderType))
                {
                    request.OrderType = request.OrderType.Trim();
                    query = query.Where(c => c.OrderType.Contains(request.OrderType));
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
    }
}
