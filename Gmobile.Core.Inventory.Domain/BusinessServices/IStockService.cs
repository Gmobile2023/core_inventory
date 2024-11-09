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
    public interface IStockService
    {
        Task<ResponseMessageBase<PagedResultDto<InventoryDto>>> GetListInventory(StockListRequest request);

        Task<ResponseMessageBase<string>> CreateInventory(StockCreatedRequest request);

        Task<ResponseMessageBase<string>> UpdateInventory(StockUpdateRequest request);

        Task<ResponseMessageBase<string>> ActiveInventory(StockActiveRequest request);

        Task<ResponseMessageBase<string>> AddSaleToInventory(StockAddSaleRequest request);

        Task<ResponseMessageBase<InventoryDto>> GetDetailInventory(int stockId);

        Task<ResponseMessageBase<PagedResultDto<SimDispalyDto>>> GetListSimInventory(StockListSimRequest request);
    }
}
