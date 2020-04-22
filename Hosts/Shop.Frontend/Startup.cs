using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shop.Frontend.Application;
using Shop.Frontend.Infrastructure;

namespace Shop.Frontend
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
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

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
