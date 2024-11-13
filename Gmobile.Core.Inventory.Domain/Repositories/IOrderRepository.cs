using Gmobile.Core.Inventory.Domain.Entities;
using Gmobile.Core.Inventory.Models.Dtos;
using Gmobile.Core.Inventory.Models.Routes.Backend;
using Inventory.Shared.Dtos.CommonDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<ResponseMessageBase<PagedResultDto<OrderDisplayDto>>> GetListOrder(OrderListRequest request);

        Task<ResponseMessageBase<OrderMessage>> OrderCreate(OrderDto orderDto, List<OrderDetailDto> items);

        Task<ResponseMessageBase<OrderMessage>> ConfirmOrder(OrderDto orderDto, OrderDescription orderDescription);

        Task<bool> UpdateOrderTotalCurrent(OrderDto orderDto, List<OrderDetailDto> orderDetails);

        Task<int> SyncToMobile(string orderCode, List<Product> products);

        Task<int> SyncToSerial(string orderCode, List<Serials> serials);

        Task<List<OrderDetailDto>> GetListOrderDetail(int orderId);

        Task<List<string>> GetListMobileToList(List<string> mobiles);

        Task<List<string>> GetListSerialToList(List<string> serials);
       
        Task<OrderDto?> GetOrderByCode(string orderCode);
    }
}
