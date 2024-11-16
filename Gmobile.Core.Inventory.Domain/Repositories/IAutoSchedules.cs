﻿using Gmobile.Core.Inventory.Models.Dtos;
using ServiceStack.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Gmobile.Core.Inventory.Models.Const;
using Microsoft.Extensions.Logging;
using Gmobile.Core.Inventory.Domain.Entities;
using Inventory.Shared.Dtos.CommonDto;
using ServiceStack;
using static Grpc.Core.ChannelOption;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Gmobile.Core.Inventory.Domain.Repositories
{
    public interface IAutoSchedules
    {
        Task ActivityLogSchedule(ActivityLogItemInit dto);
    }

    public class AutoSchedules : IAutoSchedules
    {
        private readonly ILogger<AutoSchedules> _logger;
        private readonly IStockRepository _stockRepository;
        public AutoSchedules(ILogger<AutoSchedules> logger, IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }

        public async Task ActivityLogSchedule(ActivityLogItemInit dto)
        {
            try
            {
                List<MemberDto>? dataRanger = null;
                if (dto.ActionType is ActivityLogTypeValue.CreateKitting or ActivityLogTypeValue.CreateUnKitting
                    or ActivityLogTypeValue.CreateSaleSerial or ActivityLogTypeValue.CreateSaleMobile)
                {
                    var data = await _stockRepository.GetListKitLogDetail(dto.KitingId);
                    dataRanger = ConvertInitToData(dto.ActionType, data);
                }
                else if (dto.ActionType is ActivityLogTypeValue.ConfirmSerial or ActivityLogTypeValue.ConfirmMobile)
                {
                    var data = dto.ActionType is ActivityLogTypeValue.ConfirmSerial
                        ? await _stockRepository.GetSerialListFillLog(dto.StockId, souceTransCode: dto.KeyCode)
                        : await _stockRepository.GetProductListFillLog(dto.StockId, souceTransCode: dto.KeyCode);
                    dataRanger = ConvertInitToData(dto.ActionType, data);
                }

                if (dataRanger != null && dataRanger.Count > 0)
                    await SyncActivityDetailLog(dto.LogId, dto.Description, dataRanger);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ActivityLogSchedule Exception: {ex}");

            }
        }

        /// <summary>
        /// Thực hiện ghi nhận thêm phần lịch sử chi tiết số/serial
        /// </summary>
        /// <param name="logId"></param>
        /// <param name="description"></param>
        /// <param name="dataRanger"></param>
        /// <returns></returns>
        private async Task<ResponseMessageBase<string>> SyncActivityDetailLog(long logId, string description, List<MemberDto> dataRanger)
        {
            try
            {
                var lt = dataRanger.Take(ConstTakeCount.TakeCount).ToList();
                var tmpKit = lt.Select(c => c.Number).ToList();
                dataRanger.RemoveAll(c => tmpKit.Contains(c.Number));
                int scanInt = 0;
                while (lt.Count > 0)
                {
                    _logger.LogInformation($"SyncActivityDetailLog_Step: {scanInt} - run_row = {lt.Count}");
                    var arrays = (from x in lt
                                  select new ActivityDetailLogs
                                  {
                                      LogId = (int)logId,
                                      Number = x.Number,
                                      Description = description,
                                  }).ToList();

                    var arraysSerial = (from x in lt
                                        where !string.IsNullOrEmpty(x.Serial)
                                        select new ActivityDetailLogs
                                        {
                                            LogId = (int)logId,
                                            Number = x.Serial,
                                            Description = description,
                                        }).ToList();

                    await _stockRepository.SyncActivityDetailLogs(arrays);
                    if (arraysSerial.Count > 0)
                        await _stockRepository.SyncActivityDetailLogs(arraysSerial);

                    lt = dataRanger.Take(ConstTakeCount.TakeCount).ToList();
                    tmpKit = lt.Select(c => c.Number).ToList();
                    dataRanger.RemoveAll(c => tmpKit.Contains(c.Number));
                    scanInt = scanInt + 1;
                }

                _logger.LogInformation($"SyncActivityDetailLog => Done !");
                return ResponseMessageBase<string>.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError($"SyncActivityDetailLog =>  Exception: {ex}");
                return ResponseMessageBase<string>.Error();
            }
        }

        private List<MemberDto> ConvertInitToData<T>(string actionType, List<T> data)
        {
            var item = new List<MemberDto>();
            if (actionType is ActivityLogTypeValue.CreateKitting or ActivityLogTypeValue.CreateUnKitting
                or ActivityLogTypeValue.CreateSaleSerial or ActivityLogTypeValue.CreateSaleMobile)
            {
                var dataRanger = data.ConvertTo<List<PriceKitingDetails>>();
                item = (from x in dataRanger
                        select new MemberDto
                        {
                            Number = x.Mobile,
                            Serial = x.Serial,
                            SalePrice = x.Price,
                        }).ToList();
            }
            else if (actionType is ActivityLogTypeValue.ConfirmSerial)
            {
                var dataRanger = data.ConvertTo<List<string>>();
                item = (from x in dataRanger
                        select new MemberDto
                        {
                            Number = x
                        }).ToList();
            }
            else if (actionType is ActivityLogTypeValue.ConfirmMobile)
            {
                var dataRanger = data.ConvertTo<List<string>>();
                item = (from x in dataRanger
                        select new MemberDto
                        {
                            Number = x
                        }).ToList();
            }

            return item;
        }
    }
}
