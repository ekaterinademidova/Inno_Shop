using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using System.Net;
using UsersInfrastucture.HttpClients;
using UsersInfrastucture.HttpClients.Handler;
using UsersInfrastucture.Services;
using UsersApplication.Interfaces.Data;
using UsersApplication.Interfaces.Services;
using UsersApplication.Interfaces.HttpClients;
using UsersApplication.Interfaces;

namespace UsersInfrastucture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastuctureServices
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
                            maxRetryCount: 3, // Maximum number of retries
                            maxRetryDelay: TimeSpan.FromSeconds(5), // Maximum delay between retries
                            errorNumbersToAdd: null // Optionally specify error numbers to retry
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
                client.BaseAddress = new Uri("https://localhost:5050");                
            })
            .AddHttpMessageHandler<AuthorizationHeaderHandler>();

            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
