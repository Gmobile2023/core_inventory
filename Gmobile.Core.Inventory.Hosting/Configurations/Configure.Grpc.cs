using Gmobile.Core.Inventory.Hosting.Configurations;
using ServiceStack;

[assembly: HostingStartup(typeof(ConfigureGrpc))]

namespace Gmobile.Core.Inventory.Hosting.Configurations;

public class ConfigureGrpc : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => { services.AddServiceStackGrpc(); })
            .ConfigureAppHost(appHost => { appHost.GetApp().UseRouting(); });
    }
}