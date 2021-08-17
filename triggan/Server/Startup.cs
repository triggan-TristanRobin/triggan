using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Interfaces;
using Model;
using System.Configuration;
using DataAccessLayer;
using System.Diagnostics;
using System;
using DataAccessLayer.Helpers;

namespace triggan.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Trace.TraceInformation("Configure services");

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddTransient<ISlugRepository<Post>, PostRepository>();
            services.AddTransient<ISlugRepository<Project>, Repository<Project>>();
            services.AddTransient<IRepository<Message>, MessageRepository>();

            SetDBContextService(services, Enum.Parse<DBProvider>(Configuration["DBConfig:Type"]));
        }

        public void SetDBContextService(IServiceCollection services, DBProvider dbType)
        {
            var contextName = string.Empty;
            switch (dbType)
            {
                case DBProvider.MSSQL:
                    contextName = "trigganContext";
                    Trace.TraceInformation("Retrieving MSSQL DB context with name " + contextName);
                    services.AddDbContext<TrigganContext>(options => options.UseSqlServer(Configuration.GetConnectionString(contextName)));
                    break;
                case DBProvider.Cosmos:
                    contextName = "trigganCosmos";
                    Trace.TraceInformation("Retrieving cosmos DB context with name " + contextName);
#if DEBUG
                    contextName = "trigganCosmosDebug";
#endif
                    var cosmosInfos = Configuration.GetConnectionString(contextName).Split(';');
                    services.AddDbContext<TrigganContext>(options => options.UseCosmos(Configuration.GetConnectionString(contextName), "triggandb"));

                    break;
                default:
                    // TODO: DO. Error case? I don't know mate.
                    break;
            }
            Trace.TraceInformation("Retrieved DBContext");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
