using BlazorDownloadFile;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace triggan.BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton(async p =>
            {
                var httpClient = p.GetRequiredService<HttpClient>();
                var settings = await httpClient.GetFromJsonAsync<Settings>("settings.json")
                    .ConfigureAwait(false);
                return settings;
            });
            builder.Services.AddSingleton(async p => new ContentManager(await p.GetRequiredService<Task<Settings>>(), p.GetRequiredService<HttpClient>()));
            builder.Services.AddBlazorDownloadFile();

            await builder.Build().RunAsync();
        }
    }
}