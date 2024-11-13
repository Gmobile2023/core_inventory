using Gmobile.Core.Inventory.Domain.Entities;
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Domain.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly IIventoryConnectionFactory _connectionFactory;
        private readonly ILogger<StockRepository> _logger;
        private readonly ICacheManager _cacheUil;
        public StockRepository(IIventoryConnectionFactory connectionFactory,
        ICacheManager cacheUil, ILogger<StockRepository> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _cacheUil = cacheUil;
        }


        /// <summary>
        /// Danh sách kho
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<PagedResultDto<InventoryDto>>> GetListInventory(StockListRequest request)
        {
            try
            {
                using var data = await _connectionFactory.OpenAsync();

                var query = data.From<Entities.Inventory>().Where(p => true);

                if (!string.IsNullOrEmpty(request.StockCode))
                {
                    request.StockCode = request.StockCode.Trim();
                    query = query.Where(c => c.StockCode.Contains(request.StockCode));
                }

                if (!string.IsNullOrEmpty(request.StockName))
                {
                    request.StockName = request.StockName.Trim();
                    query = query.Where(c => c.StockName.Contains(request.StockName));
                }

                if (!string.IsNullOrEmpty(request.StockName))
                {
                    request.StockName = request.StockName.Trim();
                    query = query.Where(c => c.StockName.Contains(request.StockName));
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

                var item = await data.SelectAsync(query);
                var total = await data.CountAsync(queryTotal);
                var dataView = item.ConvertTo<List<InventoryDto>>();
                dataView.ForEach(c =>
                {
                    c.Location = c.CityName ?? string.Empty;
                });
                var reponse = new PagedResultDto<InventoryDto>()
                {
                    Items = dataView,
                    TotalCount = total,
                };

                return ResponseMessageBase<PagedResultDto<InventoryDto>>.Success(reponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetInventoryList Exception : {ex}");
                return ResponseMessageBase<PagedResultDto<InventoryDto>>.Success();
            }
        }


        /// <summary>
        /// Danh sách kho Suggest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<PagedResultDto<InventorySuggestDto>>> GetSuggestInventory(StockSuggestsRequest request)
        {
            try
            {
                using var data = await _connectionFactory.OpenAsync();

                var query = data.From<Entities.Inventory>().Where(p => p.Status == InventoryStatus.Success);

                if (!string.IsNullOrEmpty(request.Suggest))
                {
                    request.Suggest = request.Suggest.Trim();
                    query = query.Where(c => c.StockName.Contains(request.Suggest) || c.StockCode.Contains(request.Suggest));
                }

                if (request.StockLevel > 0)
                {
                    query = query.Where(c => c.StockLevel == request.StockLevel);
                }

                if (request.ParentIdStock > 0)
                {
                    query = query.Where(c => c.ParentStockId == request.ParentIdStock);
                }

                var queryTotal = query;
                query = query.OrderByDescending(c => c.CreatedDate).Skip(request.SkipCount).Take(request.MaxResultCount);

                var item = await data.SelectAsync(query);
                var total = await data.CountAsync(queryTotal);
                var dataView = item.ConvertTo<List<InventorySuggestDto>>();
                var reponse = new PagedResultDto<InventorySuggestDto>()
                {
                    Items = dataView,
                    TotalCount = total,
                };

                return ResponseMessageBase<PagedResultDto<InventorySuggestDto>>.Success(reponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetSuggestInventory Exception : {ex}");
                return ResponseMessageBase<PagedResultDto<InventorySuggestDto>>.Success();
            }
        }

        /// <summary>
        /// Phần tạo kho
        /// </summary>
        /// <param name="inventoryDto"></param>
        /// <param name="roleItems"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<string>> CreateInventory(InventoryDto inventoryDto, List<InventoryRoleDto> roleItems)
        {
            using var data = await _connectionFactory.OpenAsync();
            using var trans = data.OpenTransaction();
            try
            {
                var inventory = inventoryDto.ConvertTo<Entities.Inventory>();
                var inventoryRoles = roleItems.ConvertTo<List<Entities.InventoryRoles>>();
                var id = await data.InsertAsync(inventory, true);
                inventoryRoles.ForEach(c =>
                {
                    c.StockId = (int)id;
                });

                await data.InsertAllAsync(inventoryRoles);
                trans.Commit();
                _logger.LogInformation($"CreateInventory {inventoryDto.ToJson()}. Success ");
                return ResponseMessageBase<string>.Success("Tạo kho thành công");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                _logger.LogError($"Error CreateInventory {inventoryDto.ToJson()} Exception: {ex}");
                return ResponseMessageBase<string>.Error("Tạo kho không thành công. Vui lòng kiểm tra lại thông tin !");
            }
        }

        /// <summary>
        /// Hàm sửa kho
        /// </summary>
        /// <param name="inventoryDto"></param>
        /// <param name="roleItems"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<string>> UpdateInventory(InventoryDto inventoryDto, List<InventoryRoleDto> roleItems)
        {
            using var data = await _connectionFactory.OpenAsync();
            using var trans = data.OpenTransaction();
            try
            {
                var inventory = await data.SingleByIdAsync<Entities.Inventory>(inventoryDto.Id);
                if (inventory == null)
                {
                    return ResponseMessageBase<string>.Error("Không tìm thấy kho !");
                }

                var inventoryRoles = await data.SelectAsync<Entities.InventoryRoles>(c => c.StockId == inventoryDto.Id);
                inventory.StockName = inventoryDto.StockName;
                inventory.CityName = inventoryDto.CityName;
                inventory.DistrictName = inventoryDto.DistrictName;
                inventory.WardName = inventoryDto.WardName;
                inventory.WardId = inventoryDto.WardId > 0 ? inventoryDto.WardId : null;
                inventory.DistrictId = inventoryDto.DistrictId > 0 ? inventoryDto.DistrictId : null;
                inventory.CityId = inventoryDto.CityId > 0 ? inventoryDto.CityId : null;
                await data.UpdateAsync(inventory);

                //Check vai trò và tài khoản của kho

                // var inventoryRoles = roleItems.ConvertTo<List<Entities.InventoryRoles>>();
                //var id = await data.UpdateAsync(inventory);
                //inventoryRoles.ForEach(c =>
                //{
                //    c.StockId = (int)id;
                //});

                await data.UpdateAllAsync(inventoryRoles);
                trans.Commit();
                _logger.LogInformation($"UpdateInventory {inventoryDto.ToJson()}. Success ");
                return ResponseMessageBase<string>.Success("Sửa kho thành công");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                _logger.LogError($"Error UpdateInventory {inventoryDto.ToJson()} Exception: {ex}");
                return ResponseMessageBase<string>.Error("Sửa kho không thành công. Vui lòng kiểm tra lại thông tin !");
            }
        }


        /// <summary>
        /// Kích hoạt kho
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="userActive"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<string>> ActiveInventory(int stockId, string userActive)
        {
            using var data = await _connectionFactory.OpenAsync();
            using var trans = data.OpenTransaction();
            try
            {
                var inventory = await data.SingleByIdAsync<Entities.Inventory>(stockId);
                if (inventory == null)
                {
                    return ResponseMessageBase<string>.Error("Không tìm thấy kho !");
                }

                if (inventory.IsActive)
                {
                    return ResponseMessageBase<string>.Error("Kho đã kích hoạt !");
                }

                inventory.IsActive = true;
                inventory.Status = InventoryStatus.Success;
                inventory.UserConfirm = userActive;
                inventory.ConfirmDate = DateTime.Now;
                await data.UpdateAsync(inventory);
                trans.Commit();
                _logger.LogInformation($"ActiveInventory StockId= {stockId} - UserActive= {userActive} . Success ");
                return ResponseMessageBase<string>.Success("Kích hoạt kho thành công");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                _logger.LogError($"Error ActiveInventory StockId= {stockId} - UserActive= {userActive} Exception: {ex}");
                return ResponseMessageBase<string>.Error("Sửa kho không thành công. Vui lòng kiểm tra lại thông tin !");
            }
        }


        /// <summary>
        ///Thêm người bán vào kho
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="userActive"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<string>> AddSaleToInventory(int stockId, string userSale, string userCreate)
        {
            using var data = await _connectionFactory.OpenAsync();
            using var trans = data.OpenTransaction();
            try
            {
                var inventory = await data.SingleByIdAsync<Entities.Inventory>(stockId);
                if (inventory == null)
                {
                    return ResponseMessageBase<string>.Error("Không tìm thấy kho !");
                }

                var inventoryRole = await data.SingleAsync<Entities.InventoryRoles>(c => c.StockId == stockId && c.AccountCode == userSale
                && c.RoleType == RoleType.UserSale);

                if (inventoryRole != null)
                {
                    return ResponseMessageBase<string>.Error("User đã tồn tại trong kho với vai trò người bán !");
                }

                inventoryRole = new Entities.InventoryRoles()
                {
                    AccountCode = userSale,
                    RoleType = RoleType.UserSale,
                    StockId = stockId,
                    CreatedDate = DateTime.Now,
                    UserCreated = userCreate,
                };
                await data.InsertAsync(inventory);
                trans.Commit();
                _logger.LogInformation($"AddSaleToInventory StockId= {stockId} - userSale= {userSale} - UserCreated= {userCreate} . Success ");
                return ResponseMessageBase<string>.Success("Thêm người bán thành công");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                _logger.LogError($"Error AddSaleToInventory StockId= {stockId} - userSale= {userSale} - UserCreated= {userCreate} Exception: {ex}");
                return ResponseMessageBase<string>.Error("Thêm người bán thất bại. Vui lòng kiểm tra lại thông tin !");
            }
        }


        /// <summary>
        /// Lấy chi tiết kho
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public async Task<InventoryDto?> GetInventoryDetail(int stockId)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                var inventory = await data.SingleByIdAsync<Entities.Inventory>(stockId);
                var inventoryDto = inventory.ConvertTo<InventoryDto>();
                _logger.LogInformation($"GetDetailInventory StockId= {stockId} . Success ");
                return inventoryDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error GetDetailInventory StockId= {stockId}  Exception: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Lấy chi tiết kho
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        public async Task<InventoryDto?> GetInventoryDetail(string stockCode)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                var inventory = await data.SingleAsync<Entities.Inventory>(c => c.StockCode == stockCode);
                var inventoryDto = inventory.ConvertTo<InventoryDto>();
                _logger.LogInformation($"GetDetailInventory stockCode= {stockCode} . Success ");
                return inventoryDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error GetDetailInventory stockCode= {stockCode}  Exception: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách sim/số
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<PagedResultDto<SimDispalyDto>>> GetListSimInventory(StockListSimRequest request)
        {
            try
            {
                if (request.SimType == 2)
                    return await GetListSerialInventory(request);
                else return await GetListMobileInventory(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetListSimInventory Exception : {ex}");
                return ResponseMessageBase<PagedResultDto<SimDispalyDto>>.Success();
            }
        }

        /// <summary>
        /// Danh sách số
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<ResponseMessageBase<PagedResultDto<SimDispalyDto>>> GetListMobileInventory(StockListSimRequest request)
        {
            try
            {
                using var data = await _connectionFactory.OpenAsync();

                var query = data.From<Entities.Product>().Where(p => p.StockCode == request.StockCode);

                if (request.KitingStatus != 99)
                {
                    query = query.Where(c => c.KitingStatus == request.KitingStatus);
                }

                if (!string.IsNullOrEmpty(request.Mobile))
                {
                    request.Mobile = request.Mobile.Trim();
                    query = query.Where(c => c.Mobile.Contains(request.Mobile));
                }

                if (!string.IsNullOrEmpty(request.Serial))
                {
                    request.Serial = request.Serial.Trim();
                    query = query.Where(c => c.Serial.Contains(request.Serial));
                }

                var queryTotal = query;
                query = query.OrderByDescending(c => c.CreatedDate)
                    .Skip(request.SkipCount).Take(request.MaxResultCount);

                var item = await data.SelectAsync(query);
                var total = await data.CountAsync(queryTotal);
                var dataView = item.ConvertTo<List<SimDispalyDto>>();
                var reponse = new PagedResultDto<SimDispalyDto>()
                {
                    Items = dataView,
                    TotalCount = total,
                };

                return ResponseMessageBase<PagedResultDto<SimDispalyDto>>.Success(reponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetListMobileInventory Exception : {ex}");
                return ResponseMessageBase<PagedResultDto<SimDispalyDto>>.Success();
            }
        }

        /// <summary>
        /// Lấy danh sách serial
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<ResponseMessageBase<PagedResultDto<SimDispalyDto>>> GetListSerialInventory(StockListSimRequest request)
        {
            try
            {
                using var data = await _connectionFactory.OpenAsync();

                var query = data.From<Entities.Serials>().Where(p => p.StockCode == request.StockCode);

                if (!string.IsNullOrEmpty(request.Serial))
                {
                    request.Serial = request.Serial.Trim();
                    query = query.Where(c => c.Serial.Contains(request.Serial));
                }

                var queryTotal = query;
                query = query.OrderByDescending(c => c.CreatedDate)
                    .Skip(request.SkipCount).Take(request.MaxResultCount);

                var item = await data.SelectAsync(query);
                var total = await data.CountAsync(queryTotal);
                var dataView = item.ConvertTo<List<SimDispalyDto>>();
                var reponse = new PagedResultDto<SimDispalyDto>()
                {
                    Items = dataView,
                    TotalCount = total,
                };

                return ResponseMessageBase<PagedResultDto<SimDispalyDto>>.Success(reponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetListSerialInventory Exception : {ex}");
                return ResponseMessageBase<PagedResultDto<SimDispalyDto>>.Success();
            }
        }

        /// <summary>
        /// Chi tiết của Sim/Số
        /// </summary>
        /// <param name="number"></param>
        /// <param name="simType"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<SimDispalyDto>> GetSimDetailInventory(string number, int simType)
        {
            if (simType == 2)
                return await GetSerialDetail(number);
            else return await GetMobileDetail(number);
        }

        private async Task<ResponseMessageBase<SimDispalyDto>> GetMobileDetail(string mobile)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                var product = await data.SingleAsync<Entities.Product>(c => c.Mobile == mobile);
                var simDto = product.ConvertTo<SimDispalyDto>();
                _logger.LogInformation($"GetMobileDetail Mobile= {mobile} . Success ");
                return ResponseMessageBase<SimDispalyDto>.Success(simDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error GetMobileDetail Mobile= {mobile}  Exception: {ex}");
                return ResponseMessageBase<SimDispalyDto>.Error("Không lấy được thông tin chi tiết số !");
            }
        }

        private async Task<ResponseMessageBase<SimDispalyDto>> GetSerialDetail(string serial)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                var product = await data.SingleAsync<Entities.Serials>(c => c.Serial == serial);
                var simDto = product.ConvertTo<SimDispalyDto>();
                _logger.LogInformation($"GetSerialDetail Serial= {serial} . Success ");
                return ResponseMessageBase<SimDispalyDto>.Success(simDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error GetSerialDetail Serial= {serial}  Exception: {ex}");
                return ResponseMessageBase<SimDispalyDto>.Error("Không lấy được thông tin chi tiết serial !");
            }
        }


        /// <summary>
        /// Lấy loại sản phẩm
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public async Task<CategoryDto?> GetCategoryDetail(string categoryCode)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                var inventory = await data.SingleAsync<Category>(c => c.CategoryCode == categoryCode);
                var inventoryDto = inventory.ConvertTo<CategoryDto>();
                _logger.LogInformation($"GetCategoryDetail categoryCode= {categoryCode} . Success ");
                return inventoryDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error GetCategoryDetail categoryCode= {categoryCode}  Exception: {ex}");
                return null;
            }
        }

        private async Task<ActionTypeDto?> GetActionTypeByActionType(string actionType)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                var action = await data.SingleAsync<Entities.ActionTypes>(c => c.AcitonType == actionType && c.Status == 1);
                if (action == null) return null;
                return action.ConvertTo<ActionTypeDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error GetActionTypeByActionType: actionType= {actionType}  Exception: {ex}");
                return null;
            }
        }

        private async Task<ResponseMessageBase<string>> AddLogInventoryActivitys(InventoryActivityLogDto log)
        {
            using var data = await _connectionFactory.OpenAsync();
            try
            {
                await data.InsertAsync(log.ConvertTo<InventoryActivityLogs>());
                _logger.LogInformation($"{log.OrderCode} AddLogInventoryActivitys. Success ");
                return ResponseMessageBase<string>.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error AddLogInventoryActivitys: {log.OrderCode}  Exception: {ex}");
                return ResponseMessageBase<string>.Error();
            }
        }



        /// <summary>
        /// Xử lý ghi lịch sử chung cả các dịch vụ
        /// </summary>
        /// <param name="activityLog"></param>
        /// <returns></returns>
        public async Task<ResponseMessageBase<string>> ActivitysLog(ActivityLogTypeDto activityLog)
        {
            #region 1.Validate

            var logDto = activityLog.ConvertTo<InventoryActivityLogDto>();                      
            var actionTypeDto = await GetActionTypeByActionType(activityLog.ActionType);
            if (actionTypeDto != null)
            {
                logDto.Content = string.Format(actionTypeDto.Description, activityLog.OrderCode, activityLog.SrcStockName, activityLog.DesStockName);
            }

            logDto.CreatedDate = DateTime.Now;
            logDto.Status = 1;

            #endregion

            return await AddLogInventoryActivitys(logDto);
        }
    }
}
