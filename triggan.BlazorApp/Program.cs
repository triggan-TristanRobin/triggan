using BlazorDownloadFile;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using triggan.BlazorApp.Services;
using Microsoft.Extensions.Configuration;
using triggan.BlazorApp.Helpers;

namespace triggan.BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var localClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            var settings = await localClient.GetFromJsonAsync<Settings>("settings.json");

            builder.Services.AddSingleton(sp => GetApiClient(localClient.BaseAddress, settings));
            builder.Services.AddSingleton(sp => settings);

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<IBlogService, BlogService>();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddBlazorDownloadFile();

            await builder.Build().RunAsync();
        }

        public static HttpClient GetApiClient(Uri localUri, Settings settings)
        {
            var apiUri = (settings.APIUri == null ? localUri : new Uri(settings.APIUri)).SetPort(settings.APIPort);
            return new HttpClient { BaseAddress = apiUri };
        }
    }
}