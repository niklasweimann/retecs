using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using retecs.BlazorServices;
using retecs.ReteCs.core;

namespace retecs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(
                _ => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
            builder.Services.AddSingleton(SingletonEmitter.Instance);
            builder.Services.AddSingleton(new ConnectionService());
            builder.Services.AddSingleton<BrowserService>();

            await builder.Build().RunAsync();
        }
    }
}
