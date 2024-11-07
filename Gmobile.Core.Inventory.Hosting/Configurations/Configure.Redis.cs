using Gmobile.Core.Inventory.Hosting.Configurations;
using Inventory.Shared.ServiceConfigExtentions;
using ServiceStack;
using ServiceStack.Redis;

[assembly: HostingStartup(typeof(ConfigureRedis))]

namespace Gmobile.Core.Inventory.Hosting.Configurations;

public class ConfigureRedis : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => { services.RegisterRedisSentinel(context.Configuration); })
            .ConfigureAppHost(appHost =>
            {
                appHost.GetPlugin<SharpPagesFeature>()?.ScriptMethods.Add(new RedisScripts());
            });
    }
}