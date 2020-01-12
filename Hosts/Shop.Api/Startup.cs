using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Tranquiliza.Shop.Core;
using Tranquiliza.Shop.Core.Application;
using Tranquiliza.Shop.Core.Model;
using Tranquiliza.Shop.Email;
using Tranquiliza.Shop.FileSystem;
using Tranquiliza.Shop.Sql;

namespace Tranquiliza.Shop.Api
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            var config = ConfigurationProvider.CreateFromConfig(Configuration);

            var connectionString = Configuration.GetConnectionString("ShopDatabase");
            var connectionStringProvider = new ConnectionStringProvider(connectionString);

            var key = Encoding.ASCII.GetBytes(config.SecurityKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ApplicationContextFilter));
                options.Filters.Add(typeof(RequestInformationFilter));
            });

            var seriLogger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Seq(config.SeqLoggingAddress)
                .CreateLogger();

            var logger = new LogBridge(seriLogger);

            ConfigureDependencyInjection(services, config, connectionStringProvider, logger);
        }

        private void ConfigureDependencyInjection(
            IServiceCollection services,
            IApplicationConfigurationProvider configurationProvider,
            IConnectionStringProvider connectionStringProvider,
            IApplicationLogger log)
        {
            services.AddSingleton(log);

            services.AddMediatR(typeof(DomainEntityBase));
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IEventRepository, EventRepository>();
            services.AddSingleton<ISecurity, PasswordSecurity>();
            services.AddSingleton<IDateTimeProvider, DefaultDateTimeProvider>();
            services.AddSingleton<IEventDispatcher, DefaultEventDispatcher>();
            services.AddSingleton<IMessageSender, DefaultMessageSender>();
            services.AddSingleton<IProductManagementService, ProductManagementService>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IImageRepository, ImageRepository>();
            services.AddSingleton<IInquiryManagementService, InquiryManagementService>();
            services.AddSingleton<IInquiryRepository, InquiryRepository>();

            services.AddSingleton(connectionStringProvider);
            services.AddSingleton(configurationProvider);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseCors(x => x.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
