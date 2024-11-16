using Gmobile.Core.Inventory.Domain.Entities;
using Gmobile.Core.Inventory.Models.Const;
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

        Task<ActionTypeDto?> GetActionTypeByOrderInfo(OrderTypeValue orderType, OrderSimType simType, OrderStatus orderStatus);

        Task<CategoryDto?> GetCategoryDetail(string categoryCode);
        Task<ResponseMessageBase<string>> ActivitysLog(ActivityLogTypeDto activityLog);

        Task<PriceKitingSettings> CreatePriceKitingSettings(PriceKitingSettings settingsDto);

        Task<bool> UpdatePriceKitingSettings(PriceKitingSettings settingsDto);

        Task<int> SyncKitingToMobile(int stockId, SettingType kitType, List<PriceKitingDetails> details);

        Task<List<PriceKitingDetails>> GetListKitLogDetail(long kitId);
        Task SyncActivityDetailLogs(List<ActivityDetailLogs> details);

        Task<List<SalePriceDto>> GetProductListFillLog(int stockId, string souceTransCode = "", List<string>? arrays = null);
        Task<List<SalePriceDto>> GetSerialListFillLog(int stockId, string souceTransCode = "", List<string>? arrays = null);

        Task<string> GetStockCodeNewByStockType(string stockType);

        Task<int> SyncSalePriceToSystem(int stockId, OrderSimType simType, List<SalePriceDto> salePrices, List<PriceKitingDetails> details);

        Task<List<SalePriceDto>> GetListDataTransfer(int stockId, OrderSimType simType, ObjectType objectType,
            OrderItem rangeRule, List<string> rangeItems);
    }

}
