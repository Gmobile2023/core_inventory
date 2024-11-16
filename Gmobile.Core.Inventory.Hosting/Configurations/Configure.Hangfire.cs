using Gmobile.Core.Inventory.Hosting.Configurations;
using Hangfire;
using Hangfire.PostgreSql;

[assembly: HostingStartup(typeof(ConfigureHangfire))]

namespace Gmobile.Core.Inventory.Hosting.Configurations;

public class ConfigureHangfire : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
            {
                services.AddHangfire(config =>
                {
                    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                    config.UseSimpleAssemblyNameTypeSerializer();
                    config.UseRecommendedSerializerSettings();
                    config.UsePostgreSqlStorage(context.Configuration.GetConnectionString("HangfireJob"));
                });
                services.AddHangfireServer(options =>{});
            });
    }
}