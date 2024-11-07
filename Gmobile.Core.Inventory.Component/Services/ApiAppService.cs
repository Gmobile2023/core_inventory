
using Gmobile.Core.Inventory.Models.Routes.App;
using Inventory.Shared.Dtos.CommonDto;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace Gmobile.Core.Inventory.Component.Services;

[Authenticate]
public class ApiAppService : AppServiceBase
{
    private readonly ILogger<ApiAppService> _logger;

    public ApiAppService(ILogger<ApiAppService> logger)
    {
        _logger = logger;
    }

    public async Task<ResponseMessageBase<object>> GetAsync(GetWebViewUrlRequest request)
    {
        return null;
    }
}