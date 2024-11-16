using Gmobile.Core.Inventory.Domain;
using Gmobile.Core.Inventory.Hosting.Configurations;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;

[assembly: HostingStartup(typeof(ConfigureDb))]

namespace Gmobile.Core.Inventory.Hosting.Configurations;

public class ConfigureDb : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddSingleton<IIventoryConnectionFactory>(new IventoryConnectionFactory(context.Configuration.GetConnectionString("Database"),
                PostgreSqlDialectProvider.Instance));

        }).ConfigureAppHost(appHost =>
        {
            var scriptMethods = appHost.GetPlugin<SharpPagesFeature>()?.ScriptMethods;
            scriptMethods?.Add(new DbScriptsAsync());
            using var db = appHost.Resolve<IIventoryConnectionFactory>().Open();
            
            db.CreateTableIfNotExists<Domain.Entities.Provider>();
            db.CreateTableIfNotExists<Domain.Entities.Category>();
            db.CreateTableIfNotExists<Domain.Entities.Inventory>();
            db.CreateTableIfNotExists<Domain.Entities.InventoryType>();
            db.CreateTableIfNotExists<Domain.Entities.InventoryAvailable>();
            db.CreateTableIfNotExists<Domain.Entities.Product>();
            db.CreateTableIfNotExists<Domain.Entities.Serials>();            
            db.CreateTableIfNotExists<Domain.Entities.OrderTypes>();
            db.CreateTableIfNotExists<Domain.Entities.Order>();
            db.CreateTableIfNotExists<Domain.Entities.OrderDetails>();
            db.CreateTableIfNotExists<Domain.Entities.OrderDescription>();
            db.CreateTableIfNotExists<Domain.Entities.SimDetails>();
            db.CreateTableIfNotExists<Domain.Entities.ActionTypes>();
            db.CreateTableIfNotExists<Domain.Entities.InventoryActivityLogs>();
            db.CreateTableIfNotExists<Domain.Entities.ActivityDetailLogs>();
            db.CreateTableIfNotExists<Domain.Entities.InventorySettings>();
            db.CreateTableIfNotExists<Domain.Entities.InventoryRoles>();
            db.CreateTableIfNotExists<Domain.Entities.PriceKitingSettings>();
            db.CreateTableIfNotExists<Domain.Entities.PriceKitingDetails>();


            OrmLiteConfig.DialectProvider.GetStringConverter().UseUnicode = true;
            OrmLiteConfig.InsertFilter = (dbCmd, row) =>
            {
                if (row is AuditBase auditRow)
                {
                    auditRow.CreatedDate = DateTime.UtcNow;
                    auditRow.ModifiedDate = DateTime.UtcNow;
                }
            };
            OrmLiteConfig.UpdateFilter = (dbCmd, row) =>
            {
                if (row is AuditBase auditRow)
                    auditRow.ModifiedDate = DateTime.UtcNow;
            };
        });
    }
}