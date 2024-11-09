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
    public interface IStockRepository
    {
        Task<ResponseMessageBase<PagedResultDto<InventoryDto>>> GetListInventory(StockListRequest request);

        Task<ResponseMessageBase<string>> CreateInventory(InventoryDto inventoryDto, List<InventoryRoleDto> roleItems);

        Task<ResponseMessageBase<string>> UpdateInventory(InventoryDto inventoryDto, List<InventoryRoleDto> roleItems);

        Task<ResponseMessageBase<string>> ActiveInventory(int stockId, string userActive);

        Task<ResponseMessageBase<string>> AddSaleToInventory(int stockId, string userSale, string userCreate);

        Task<ResponseMessageBase<InventoryDto>> GetDetailInventory(int stockId);

        Task<ResponseMessageBase<PagedResultDto<SimDispalyDto>>> GetListSimInventory(StockListSimRequest request);
    }
}
