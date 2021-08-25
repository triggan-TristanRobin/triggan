using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics;
using triggan.BlogManager;
using triggan.BlogManager.Helpers;
using triggan.BlogManager.Interfaces;
using triggan.BlogModel;

namespace triggan.API
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

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExpensesTracker", Version = "v1" });
            });

            services.AddTransient<BlogAccessor>();

            var conf = Configuration["DBConfig:Type"];
            SetDBContextService(services, Enum.Parse<DBProvider>(conf));
        }

        public void SetDBContextService(IServiceCollection services, DBProvider dbType)
        {
            var contextName = string.Empty;
            switch (dbType)
            {
                case DBProvider.MSSQL:
                    contextName = "trigganContext";
                    Trace.TraceInformation("Retrieving MSSQL DB context with name " + contextName);
                    services.AddDbContext<TrigganContext>(options => options.UseSqlServer(Configuration.GetConnectionString(contextName)).LogTo(Console.WriteLine, LogLevel.Information));
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

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpensesTracker v1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}