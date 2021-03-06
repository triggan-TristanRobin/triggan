using BlazorDownloadFile;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace triggan.Client
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
                settings.UseLocal = true;
                return settings;
            });
            builder.Services.AddSingleton(async p => new ContentManager(await p.GetRequiredService<Task<Settings>>(), p.GetRequiredService<HttpClient>()));
            builder.Services.AddBlazorDownloadFile();

            await builder.Build().RunAsync();
        }
    }
}
