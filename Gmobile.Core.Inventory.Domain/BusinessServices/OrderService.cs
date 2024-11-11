using Gmobile.Core.Inventory.Domain.Repositories;
using Gmobile.Core.Inventory.Models.Const;
using Gmobile.Core.Inventory.Models.Dtos;
using Gmobile.Core.Inventory.Models.Routes.Backend;
using Inventory.Shared.Dtos.CommonDto;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Domain.BusinessServices
{
    public class OrderService : IOrderService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IOrderRepository _orderRepository;
        public OrderService(IStockRepository stockRepository, IOrderRepository orderRepository)
        {
            _stockRepository = stockRepository;
            _orderRepository = orderRepository;
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

            request.UserCreated = (request.UserCreated ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(request.UserCreated))
            {
                return ResponseMessageBase<OrderMessage>.Error("Quý khách chưa người thực hiện");
            }

            if (request.Items == null || request.Items.Count <= 0)
            {
                return ResponseMessageBase<OrderMessage>.Error("Chi tiết đơn hàng đang để trống.");
            }

            //Xử lý check kỹ thông tin đơn hàng
            foreach (var item in request.Items)
            {

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
                OrderType = OrderValueType.Import,
                OrderTitle = request.Title,
                Description = request.Description,
                UserCreated = request.UserCreated,
                CreatedDate = DateTime.Now,                
            };

            var items = new List<OrderDetailDto>()
            {

            };


            var messagerOrder = await _orderRepository.OrderCreate(orderDto, items);

            #endregion

            return messagerOrder;
        }
    }
}
