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

        Task<ResponseMessageBase<PagedResultDto<InventorySuggestDto>>> GetSuggestInventory(StockSuggestsRequest request);

        Task<ResponseMessageBase<string>> CreateInventory(InventoryDto inventoryDto, List<InventoryRoleDto> roleItems);

        Task<ResponseMessageBase<string>> UpdateInventory(InventoryDto inventoryDto, List<InventoryRoleDto> roleItems);

        Task<ResponseMessageBase<string>> ActiveInventory(int stockId, string userActive);

        Task<ResponseMessageBase<string>> AddSaleToInventory(int stockId, string userSale, string userCreate);

        Task<InventoryDto?> GetInventoryDetail(int stockId);

        Task<InventoryDto?> GetInventoryDetail(string stockCode);

        Task<ResponseMessageBase<PagedResultDto<SimDispalyDto>>> GetListSimInventory(StockListSimRequest request);

        Task<ResponseMessageBase<SimDispalyDto>> GetSimDetailInventory(string number, int simType);

        Task<ActionTypeDto?> GetActionTypeByActionType(string actionType);
        Task<ResponseMessageBase<string>> AddLogInventoryActivitys(InventoryActivityLogDto log);
    }
}
