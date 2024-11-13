using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UsersInfrastructure.HttpClients;
using UsersInfrastructure.HttpClients.Handler;
using UsersInfrastructure.Services;
using UsersApplication.Interfaces.Data;
using UsersApplication.Interfaces.ServiceContracts;
using UsersApplication.Interfaces.HttpClientContracts;
using UsersApplication.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UsersInfrastructure.Data.Interceptors;
using UsersApplication.Interfaces.WrappersContracts;
using UsersInfrastructure.Wrappers;

namespace UsersInfrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            // Add services to the container.

            // DbContext and Interceptors
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(
                    connectionString,
                    sqlServerOptions => {
                        sqlServerOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorNumbersToAdd: null
                        );
                        sqlServerOptions.CommandTimeout(60);
                    }
                );
            });

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // HttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // HTTP Clients Handlers
            services.AddTransient<AuthorizationHeaderHandler>();

            // HTTP Clients
            services.AddHttpClient<IProductsServiceClient, ProductsServiceClient>((serviceProvider, client) =>
            {
                client.Timeout = TimeSpan.FromSeconds(200);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.BaseAddress = new Uri("https://localhost:5050"); // must to be changed for docker       
            })
            .AddHttpMessageHandler<AuthorizationHeaderHandler>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISmtpClientWrapper, SmtpClientWrapper>();

            return services;
        }
    }
}
