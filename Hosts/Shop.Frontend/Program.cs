using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Shop.Frontend.Application;
using Shop.Frontend.Infrastructure;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shop.Frontend
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.ConfigureServices();
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IConfiguration, Configuration>();
            services.AddSingleton<IBasketService, BasketService>();
            services.AddSingleton<IApplicationState, ApplicationState>();
            services.AddSingleton<IApplicationStateManager, ApplicationStateManager>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IApiGateway, ApiGateway>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IInquiryService, InquiryService>();
        }
    }
}
