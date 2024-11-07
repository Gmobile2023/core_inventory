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
            
            db.CreateTableIfNotExists<Gmobile.Core.Inventory.Domain.Entities.Provider>();
            db.CreateTableIfNotExists<Gmobile.Core.Inventory.Domain.Entities.Order>();
            
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