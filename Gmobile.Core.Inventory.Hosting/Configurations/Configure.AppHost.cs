using Funq;
using Gmobile.Core.Inventory.Component.Connectors;
using Gmobile.Core.Inventory.Component.Services;
using Gmobile.Core.Inventory.Domain.BusinessServices;
using Gmobile.Core.Inventory.Domain.Repositories;
using Gmobile.Core.Inventory.Hosting.Configurations;
using Gmobile.Core.Inventory.Models.Const;
using Inventory.Shared.CacheManager;
using ServiceStack;
using ServiceStack.Api.OpenApi;
using ServiceStack.Text;
using HostConfig = ServiceStack.HostConfig;

[assembly: HostingStartup(typeof(AppHost))]

namespace Gmobile.Core.Inventory.Hosting.Configurations;

public class AppHost() : AppHostBase("gmobile_inventory", typeof(MainService).Assembly), IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder
            .ConfigureServices((context, services) =>
            {
                // services.AddSingleton<ICacheClient>(c => c.res<IRedisClientsManager>().GetCacheClient());

                services.AddOptions<HostOptions>()                              
                    .Configure(options => options.ShutdownTimeout = TimeSpan.FromMinutes(1));
                services.AddScoped<ICacheManager, CacheManager>();
                services.AddScoped<IStockRepository, StockRepository>();
                services.AddScoped<IOrderRepository, OrderRepository>();
               // services.AddScoped<ITransCodeGenerator, TransCodeGenerator>();
                services.AddScoped<IStockService, StockService>();
                services.AddScoped<IOrderService, OrderService>();
            })
            .ConfigureAppHost(appHost => { })
            .Configure((context, app) =>
            {
                app.UseAuthentication();
                if (!HasInit)
                    app.UseServiceStack(new AppHost());
                var pathBase = context.Configuration["PATH_BASE"];
                if (!string.IsNullOrEmpty(pathBase)) app.UsePathBase(pathBase);
                app.UseRouting();
            });
    }

    public override void Configure(Container container)
    {
        SetConfig(new HostConfig
        {
            DefaultContentType = MimeTypes.Json,
            DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false),
            UseSameSiteCookies = true,
            GlobalResponseHeaders = new Dictionary<string, string>
            {
                { "Server", "nginx/1.4.7" },
                { "Vary", "Accept" },
                { "X-Powered-By", "igmobile" }
            },
            EnableFeatures = Feature.All.Remove(
                Feature.Csv | Feature.Soap11 | Feature.Soap12) // | Feature.Metadata),
        });
        ConfigurePlugin<PredefinedRoutesFeature>(feature => feature.JsonApiRoute = null);
        Plugins.Add(new GrpcFeature(App));
        Plugins.Add(new OpenApiFeature());

        JsConfig.Init(new Config
        {
            ExcludeTypeInfo = true,
            AssumeUtc = true,
            TreatEnumAsInteger = true
        });

        JsConfig<DateTime?>.SerializeFn = time =>
        {
            if (time == null) return null;
            return time.Value.Kind == DateTimeKind.Local
                ? time.Value.ToString("o")
                : TimeZoneInfo.ConvertTime(time.Value, TimeZoneInfo.Local).ToString("o");
        };

        JsConfig<DateTime>.SerializeFn = time => time.Kind == DateTimeKind.Local
            ? time.ToString("o")
            : TimeZoneInfo.ConvertTime(time, TimeZoneInfo.Local).ToString("o");
    }
}