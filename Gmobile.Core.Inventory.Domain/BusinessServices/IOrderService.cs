using Gmobile.Core.Inventory.Models.Dtos;
using Gmobile.Core.Inventory.Models.Routes.Backend;
using Inventory.Shared.Dtos.CommonDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Domain.BusinessServices
{
    public interface IOrderService
    {
        Task<ResponseMessageBase<PagedResultDto<OrderDisplayDto>>> GetListOrder(OrderListRequest request);

        Task<ResponseMessageBase<OrderMessage>> OrderCreate(OrderCreatedRequest request);

        Task<ResponseMessageBase<OrderMessage>> ConfirmOrder(OrderConfirmRequest request);

        Task<ResponseMessageBase<OrderMessage>> OrderTransferCreate(OrderTransferCreatedRequest request);
    }
}
