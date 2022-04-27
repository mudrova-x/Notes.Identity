using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Notes.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Notes.Identity.Models;

namespace Notes.Identity
{
    public class Startup
    {
        public IConfiguration AppConfiguration { get; }
        public Startup(IConfiguration configuration) =>
            AppConfiguration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = AppConfiguration.GetValue<string>("DbConnection");

            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            services.AddIdentity<AppUser, IdentityRole>(config =>
            {
                // настройка требований
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                // храним токены и пути
                config.Cookie.Name = "Notes.Identity.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            services.AddIdentityServer()
                .AddAspNetIdentity<AppUser>()
                .AddInMemoryApiResources(Configuration.ApiResources)
                .AddInMemoryIdentityResources(Configuration.IdentityResources)
                .AddInMemoryApiScopes(Configuration.ApiScopes)
                .AddInMemoryClients(Configuration.Clients)
                .AddDeveloperSigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            // для начала обработки маршрутиризации
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
