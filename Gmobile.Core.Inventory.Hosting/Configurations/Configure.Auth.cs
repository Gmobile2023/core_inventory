
using Gmobile.Core.Inventory.Hosting.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ServiceStack;
using ServiceStack.Auth;
using System.IdentityModel.Tokens.Jwt;
using Inventory.Shared.Dtos.CommonDto;

[assembly: HostingStartup(typeof(ConfigureAuth))]
namespace Gmobile.Core.Inventory.Hosting.Configurations;

public class ConfigureAuth : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder
            .ConfigureServices((context, services) =>
            {
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    if (context.Configuration == null) return;
                    options.Authority = context.Configuration[$"OAuth:IdentityServer:AuthorizeUrl"];
                    options.RequireHttpsMetadata = false;
                    options.Audience = context.Configuration["OAuth:IdentityServer:Audience"];
                });
            })
            .ConfigureAppHost(appHost =>
            {
                var appSettings = appHost.AppSettings;
                appHost.Plugins.Add(new AuthFeature(() => new CustomUserSession(),
                    new IAuthProvider[]
                    {
                        new NetCoreIdentityAuthProvider(appSettings)
                        {
                            PopulateSessionFilter = (session, token, req) =>
                            {
                                //var accountType=SystemAccountType.Default;
                                var userAccountTypeClaim = token.Claims.FirstOrDefault(p => p.Type == "account_type")?.Value;
                                if (!string.IsNullOrEmpty(userAccountTypeClaim))
                                {
                                    //Enum.TryParse(userAccountTypeClaim, out accountType);
                                }
                                ((CustomUserSession)session).AccountCode = token.Claims.FirstOrDefault(p => p.Type == "account_code")?.Value;
                                ((CustomUserSession)session).ClientId = token.Claims.FirstOrDefault(p => p.Type == "client_id")?.Value;
                                ((CustomUserSession)session).FullName = token.Claims.FirstOrDefault(p => p.Type == "full_name")?.Value;
                                ((CustomUserSession)session).FirstName = token.Claims.FirstOrDefault(p => p.Type == "sur_name")?.Value;
                                ((CustomUserSession)session).LastName = token.Claims.FirstOrDefault(p => p.Type == "name")?.Value;
                                ((CustomUserSession)session).Email = token.Claims.FirstOrDefault(p => p.Type == "email_address")?.Value;
                                ((CustomUserSession)session).PhoneNumber = token.Claims.FirstOrDefault(p => p.Type == "phone_number")?.Value;
                                ((CustomUserSession)session).PhoneNumberOtp = token.Claims.FirstOrDefault(p => p.Type == "phone_number_otp")?.Value;
                                ((CustomUserSession)session).ParentCode = token.Claims.FirstOrDefault(p => p.Type == "parent_code")?.Value;
                                //((CustomUserSession)session).AccountType = accountType;
                            }
                        }
                    })
                );
            });
    }
}