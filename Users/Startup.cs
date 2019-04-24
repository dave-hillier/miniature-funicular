using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using ScopeClaim;
using Users.Model;

[assembly: InternalsVisibleTo("Users.Tests")]

namespace Users
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
              {
                  options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                  options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
              })
              .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHealthChecks();
            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            services.TryAddScoped<ITenantAccessor, TenantAccessor>();
            services.TryAddScoped<IUsernameAccessor, UsernameAccessor>();

            var audience = Configuration["Authentication:Audience"];
            var authority = Configuration["Authentication:Authority"];

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = authority;
                    options.Audience = audience;
                    options.SaveToken = true;
                });

            services.AddAuthorization(options =>
            {
                options.AddScopePolicies(new[] { "read:users" });
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Default")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHealthChecks("/status/health");

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
