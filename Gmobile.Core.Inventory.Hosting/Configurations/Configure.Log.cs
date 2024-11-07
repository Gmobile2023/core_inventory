using Gmobile.Core.Inventory.Hosting.Configurations;
using Inventory.Shared.ServiceConfigExtentions;

[assembly: HostingStartup(typeof(ConfigureLog))]

namespace Gmobile.Core.Inventory.Hosting.Configurations;

public class ConfigureLog : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => { services.RegisterLogging(context.Configuration); });
    }
}