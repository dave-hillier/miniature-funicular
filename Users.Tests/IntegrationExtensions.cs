using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Users.Model;

namespace Users.Tests
{
    internal static class IntegrationExtensions
    {
        public static void ConfigureInMemoryDatabases(this IServiceCollection services, InMemoryDatabaseRoot memoryDatabaseRoot)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Application", memoryDatabaseRoot));
        }

        public static void ConfigureUnvalidatedAuth(this IServiceCollection services)
        {
            services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireSignedTokens = false,
                    RequireExpirationTime = false,
                    ValidateActor = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateTokenReplay = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false,
                    SignatureValidator = (token, parameters) => new JwtSecurityToken(token),
                };
                options.RequireHttpsMetadata = false;
                options.Authority = null;
            });
        }
        
        public static IWebHostBuilder ConfigureTest(this IWebHostBuilder builder)
        {
            var inMemoryDatabaseRoot = new InMemoryDatabaseRoot();
            return builder.UseEnvironment("Test")
                .UseSetting("Authentication:Audience", "https://audience")
                .UseSetting("Authentication:Authority", "https://authority")
                .ConfigureServices(services =>
                {
                    services.ConfigureInMemoryDatabases(inMemoryDatabaseRoot);
                    services.ConfigureUnvalidatedAuth();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .UseStartup<Startup>();
        }
    }
}
