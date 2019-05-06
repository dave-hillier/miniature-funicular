﻿using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Properties.Model;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ScopeClaim;

[assembly:InternalsVisibleTo("Properties.Tests")]

namespace Properties
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

            services
                .AddHealthChecks();
            services.AddHttpContextAccessor();


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
                options.AddScopePolicies("read:properties", 
                    "update:properties", "update:property", "create:property", "delete:property");
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            services.TryAddScoped<ITenantAccessor, TenantAccessor>();
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