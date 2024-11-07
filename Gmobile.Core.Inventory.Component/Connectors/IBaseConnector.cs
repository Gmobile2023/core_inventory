using Gmobile.Core.Inventory.Domain.Entities;
using Inventory.Shared.Dtos.CommonDto;

namespace Gmobile.Core.Inventory.Component.Connectors;

public interface IBaseConnector
{
    Task<ResponseMessageBase<object>> GetOrderInfo(Provider provider, string orderCode);
    Task<ResponseMessageBase<object>> IpnRequest(Provider provider, Order order);
}