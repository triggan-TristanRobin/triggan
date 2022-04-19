using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics;
using System.Text;
using triggan.API.Helpers;
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "triggan", Version = "v1" });
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            services.AddTransient<BlogAccessor>();

            var conf = Configuration["DBConfig:Type"];
            SetDBContextService(services, Enum.Parse<DBProvider>(conf));
            AddAuthenticationServices(services);
        }

        private void AddAuthenticationServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            var key = Encoding.ASCII.GetBytes(appSettingsSection.Get<AppSettings>().JwtSecret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "triggan API v1"));
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
            app.UseAuthentication();
            app.UseAuthorization();
            loggerFactory.AddLog4Net();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}