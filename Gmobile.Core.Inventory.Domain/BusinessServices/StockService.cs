using Gmobile.Core.Inventory.Domain.Repositories;
using Gmobile.Core.Inventory.Models.Const;
using Gmobile.Core.Inventory.Models.Dtos;
using Gmobile.Core.Inventory.Models.Routes.Backend;
using Inventory.Shared.Const;
using Inventory.Shared.Dtos.CommonDto;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Domain.BusinessServices
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<StockService> _logger;
        public StockService(IStockRepository stockRepository, ILogger<StockService> logger)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }

        /// <summary>
        /// Danh sách kho hiển thị Grid
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<PagedResultDto<InventoryDto>>> GetListInventory(StockListRequest request)
        {
            return await _stockRepository.GetListInventory(request);
        }

        /// <summary>
        /// Danh sách kho Suggests
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<PagedResultDto<InventorySuggestDto>>> GetSuggestsInventory(StockSuggestsRequest request)
        {
            return await _stockRepository.GetSuggestInventory(request);
        }

        /// <summary>
        /// Hàm tạo mới kho
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<string>> CreateInventory(StockCreatedRequest request)
        {
            #region 1.Validate

            request.StockCode = (request.StockCode ?? string.Empty).Trim();
            request.StockName = (request.StockName ?? string.Empty).Trim();
            request.StockType = (request.StockType ?? string.Empty).Trim();
            request.CityName = (request.CityName ?? string.Empty).Trim();
            request.DistrictName = (request.DistrictName ?? string.Empty).Trim();
            request.WardName = (request.WardName ?? string.Empty).Trim();
            request.Address = (request.Address ?? string.Empty).Trim();
            request.UserCreated = (request.UserCreated ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(request.StockName))
                return ResponseMessageBase<string>.Error(ResponseCodeConst.EmptyName, "Tên kho không được để trống");

            if (string.IsNullOrEmpty(request.StockType))
                return ResponseMessageBase<string>.Error(ResponseCodeConst.EmptyLevel, "Cấp kho không được để trống");


            if (string.IsNullOrEmpty(request.UserCreated))
                return ResponseMessageBase<string>.Error(ResponseCodeConst.EmptyLevel, "Người tạo kho không được để trống");

            if (request.StockLevel > 1)
            {
                if (request.ParentId <= 0)
                    return ResponseMessageBase<string>.Error(ResponseCodeConst.EmptyLevel, "Quý khách chưa truyền kho cha.");
            }

            if (request.RoleTypes == null || request.RoleTypes.Count <= 0)
                return ResponseMessageBase<string>.Error(ResponseCodeConst.ExportRecover, "Chứa truyền người xuất và thu hồi");

            #endregion

            ///Xử lý thêm phần mã kho??
            var inventoryDto = request.ConvertTo<InventoryDto>();
            if (inventoryDto.ParentStockId > 0)
            {
                var parentStock = await _stockRepository.GetInventoryDetail(inventoryDto.ParentStockId ?? 0);
                if (parentStock == null)
                {
                    return ResponseMessageBase<string>.Error("Kiểm tra lại thông tin kho cha.");
                }
                inventoryDto.TreePath = parentStock.StockCode + "-" + inventoryDto.StockCode;
            }
            else inventoryDto.TreePath = inventoryDto.StockCode;

            inventoryDto.CreatedDate = DateTime.Now;
            var roleItems = request.RoleTypes.ConvertTo<List<InventoryRoleDto>>();

            var reponse = await _stockRepository.CreateInventory(inventoryDto, roleItems);
            if (reponse.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
                await _stockRepository.ActivitysLog(new ActivityLogTypeDto()
                {
                    ActionType = ActivityLogTypeValue.CreateStock,
                    StockLevel = inventoryDto.StockType,
                    DesStockName = request.StockName,
                    UserCreated = request.UserCreated,
                });
            return reponse;
        }

        /// <summary>
        /// Hàm sửa kho
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<string>> UpdateInventory(StockUpdateRequest request)
        {
            #region 1.Validate

            request.StockName = (request.StockName ?? string.Empty).Trim();
            request.StockType = (request.StockType ?? string.Empty).Trim();
            request.CityName = (request.CityName ?? string.Empty).Trim();
            request.DistrictName = (request.DistrictName ?? string.Empty).Trim();
            request.WardName = (request.WardName ?? string.Empty).Trim();
            request.Address = (request.Address ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(request.StockName))
                return ResponseMessageBase<string>.Error(ResponseCodeConst.EmptyName, "Tên kho không được để trống");

            if (string.IsNullOrEmpty(request.StockType))
                return ResponseMessageBase<string>.Error(ResponseCodeConst.EmptyLevel, "Cấp kho không được để trống");

            if (request.RoleTypes == null || request.RoleTypes.Count <= 0)
                return ResponseMessageBase<string>.Error(ResponseCodeConst.ExportRecover, "Chứa truyền người xuất và thu hồi");

            #endregion

            ///Xử lý thêm phần mã kho??
            var inventoryDto = request.ConvertTo<InventoryDto>();
            inventoryDto.CreatedDate = DateTime.Now;
            inventoryDto.UserConfirm = request.UserCreated;
            var roleItems = request.RoleTypes.ConvertTo<List<InventoryRoleDto>>();

            var reponse = await _stockRepository.UpdateInventory(inventoryDto, roleItems);
            if (reponse.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
                await _stockRepository.ActivitysLog(new ActivityLogTypeDto()
                {
                    ActionType = ActivityLogTypeValue.EditStock,
                    StockLevel = inventoryDto.StockType,
                    DesStockName = request.StockName,
                    UserCreated = request.UserCreated,
                });

            return reponse;
        }

        /// <summary>
        /// Kích hoạt kho
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<string>> ActiveInventory(StockActiveRequest request)
        {
            #region 1.Validate
            request.UserActive = (request.UserActive ?? string.Empty).Trim();

            if (request.Id <= 0)
                return ResponseMessageBase<string>.Error("Quý khách chưa truyền Id của kho");

            if (string.IsNullOrEmpty(request.UserActive))
                return ResponseMessageBase<string>.Error("User thực hiện kích hoạt không được để trống");

            #endregion
            var inventoryDto = await _stockRepository.GetInventoryDetail(request.Id);
            if (inventoryDto == null)
            {
                return ResponseMessageBase<string>.Error("Kiểm tra lại thông tin kho.");
            }

            var reponse = await _stockRepository.ActiveInventory(request.Id, request.UserActive);
            if (reponse.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
                await _stockRepository.ActivitysLog(new ActivityLogTypeDto()
                {
                    ActionType = ActivityLogTypeValue.ActiveStock,
                    StockLevel = inventoryDto.StockLevel.ToString(),
                    DesStockName = inventoryDto.StockName,
                    UserCreated = request.UserActive,
                });

            return reponse;

        }

        /// <summary>
        /// Thêm người bán hàng vào kho
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<string>> AddSaleToInventory(StockAddSaleRequest request)
        {
            #region 1.Validate
            request.UserCreate = (request.UserCreate ?? string.Empty).Trim();
            request.UserSale = (request.UserSale ?? string.Empty).Trim();

            if (request.Id <= 0)
                return ResponseMessageBase<string>.Error("Quý khách chưa truyền Id của kho");

            if (string.IsNullOrEmpty(request.UserCreate))
                return ResponseMessageBase<string>.Error("User thực hiện không được để trống");

            if (string.IsNullOrEmpty(request.UserSale))
                return ResponseMessageBase<string>.Error("User bán hàng không được để trống");

            #endregion

            var inventoryDto = await _stockRepository.GetInventoryDetail(request.Id);
            if (inventoryDto == null)
            {
                return ResponseMessageBase<string>.Error("Kiểm tra lại thông tin kho.");
            }

            var reponse = await _stockRepository.AddSaleToInventory(request.Id, request.UserSale, request.UserCreate);
            if (reponse.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
                await _stockRepository.ActivitysLog(new ActivityLogTypeDto()
                {
                    ActionType = ActivityLogTypeValue.AccountToStock,
                    StockLevel = inventoryDto.StockLevel.ToString(),
                    DesStockName = inventoryDto.StockName,
                    UserCreated = request.UserCreate,
                });

            return reponse;

        }

        /// <summary>
        /// Chi tiết kho 
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<InventoryDto?>> GetDetailInventory(int stockId)
        {
            #region 1.Validate

            if (stockId <= 0)
                return ResponseMessageBase<InventoryDto?>.Error("Quý khách chưa truyền Id của kho");

            #endregion

            var inventoryDto = await _stockRepository.GetInventoryDetail(stockId);
            return ResponseMessageBase<InventoryDto?>.Success(inventoryDto);
        }

        /// <summary>
        /// Lấy danh sách sim/số
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<PagedResultDto<SimDispalyDto>>> GetListSimInventory(StockListSimRequest request)
        {
            return await _stockRepository.GetListSimInventory(request);
        }

        /// <summary>
        /// Chi tiết sim/số
        /// </summary>
        /// <param name="number"></param>
        /// <param name="simType"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<SimDispalyDto>> GetSimDetailInventory(string number, int simType)
        {
            #region 1.Validate

            if (string.IsNullOrEmpty(number))
                return ResponseMessageBase<SimDispalyDto>.Error("Số/Serial không được để trống");

            if (simType <= 0)
                return ResponseMessageBase<SimDispalyDto>.Error("Quý khách cần truyền loại sim hoặc số");

            #endregion

            return await _stockRepository.GetSimDetailInventory(number, simType);
        }
    }
}
