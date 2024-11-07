using Gmobile.Core.Inventory.Hosting.Configurations;
using Gmobile.Core.Inventory.Models.Routes.App;
using Gmobile.Core.Inventory.Models.Validation;
using ServiceStack.FluentValidation;

[assembly: HostingStartup(typeof(ConfigureValidator))]

namespace Gmobile.Core.Inventory.Hosting.Configurations;

public class ConfigureValidator : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddTransient<IValidator<GetWebViewUrlRequest>, GetWebViewUrlRequestValidator>();
        });
    }
}