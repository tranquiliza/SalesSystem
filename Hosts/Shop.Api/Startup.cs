using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Tranquiliza.Shop.Core;
using Tranquiliza.Shop.Core.Application;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            var config = ConfigurationProvider.CreateFromConfig(Configuration);

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

            ConfigureDependencyInjection(services, config);
        }

        private void ConfigureDependencyInjection(IServiceCollection services, IConfigurationProvider configurationProvider)
        {
            services.AddTransient<IUserService, UserService>();

            services.AddSingleton<IUserRepository, UserRepMock>();
            services.AddSingleton<ISecurity, PasswordSecurity>();
            services.AddSingleton<IDateTimeProvider, DefaultDateTimeProvider>();

            services.AddSingleton(configurationProvider);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

    internal class UserRepMock : IUserRepository
    {
        private List<User> _users = new List<User>();

        public UserRepMock()
        {
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<User> Get(Guid id)
        {
            return Task.FromResult(_users.First(x => x.Id == id));
        }

        public Task<User> GetByEmail(string email)
        {
            var user = _users.Find(user => string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        public Task Save(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }
    }
}
