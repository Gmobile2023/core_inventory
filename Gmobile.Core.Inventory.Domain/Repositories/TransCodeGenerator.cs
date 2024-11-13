using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServiceStack.Redis;

namespace Gmobile.Core.Inventory.Domain.Repositories;

public class TransCodeGenerator : ITransCodeGenerator
{
    private readonly ILogger<TransCodeGenerator> _logger;
    private readonly IRedisClientsManager _redisClientsManager;

    public TransCodeGenerator(ILogger<TransCodeGenerator> logger, IRedisClientsManager redisClientsManager)
    {
        _logger = logger;
        _redisClientsManager = redisClientsManager;
    }

    public async Task<string> TransCodeGeneratorAsync(string prefix = "P")
    {
        try
        {
            var dateParam = DateTime.Now.ToString("yyMMdd");
            await using var client = await _redisClientsManager.GetClientAsync();
            var key = $"Gmobile_TransCode:{prefix + dateParam}";
            var id = await client.IncrementValueAsync(key);
            if (id == 1) //Bắt đầu ngày mới
            {
                var oldkey = $"Gmobile_TransCode:{prefix + DateTime.Now.AddDays(-1).ToString("yyMMdd")}";
                await client.RemoveAsync(oldkey); //xóa key cũ
            }

            return await Task.FromResult(prefix + dateParam + id.ToString().PadLeft(8, '0'));
        }
        catch (Exception ex)
        {
            _logger.LogError("TransCode.GeneratorAsync error: {Error}", ex.Message);
        }

        var rand = new Random();
        var date = DateTime.Now.ToString("yy");
        return await Task.FromResult(prefix + date + rand.Next(000000000, 999999999));
    }
   
}