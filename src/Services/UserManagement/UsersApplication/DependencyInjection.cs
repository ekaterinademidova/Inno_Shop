using BuildingBlocks.Behaviors;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using UsersApplication.Interfaces.Services;
using UsersApplication.Models;
using UsersInfrastucture.Services;

namespace UsersApplication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            // Configuration of Email settings
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>()
                ?? throw new InvalidOperationException("Email settings are not configured properly.");
            services.AddSingleton(emailSettings);



            // Configuration of JWT settings
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>() 
                ?? throw new InvalidOperationException("JWT settings are not configured properly.");
            services.AddSingleton(jwtSettings);

            // Configuration of JWT authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  //???
            })
            .AddJwtBearer(options =>
            {                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = jwtSettings.GetSymmetricSecurityKey()
                };
            });
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddAuthorization();

            //services.AddHttpContextAccessor();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            //services.AddFeatureManagement();
            //services.AddMessageBroker(configuration);

            return services;
        }
    }
}
