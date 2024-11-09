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
        public async Task<PagedResultDto<InventoryDto>> GetListInventory(StockListRequest request)
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
                return new PagedResultDto<InventoryDto>()
                {
                    Items = dataView,
                    TotalCount = total,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetInventoryList Exception : {ex}");
                return new PagedResultDto<InventoryDto>();
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

    }
}
