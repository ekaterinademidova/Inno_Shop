using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UsersApplication.Interfaces.ServiceContracts;
using UsersDomain.Constants;
using UsersInfrastructure.Data;
using UsersInfrastructure.Data.Extensions;

namespace UsersAPI.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public string? CurrentUserJwtToken { get; set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("IntegrationTests");

            builder.ConfigureServices(services => { 
                var descriptor = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                var authService = scopedServices.GetRequiredService<IAuthenticationService>();

                db.Database.EnsureCreated();
                db.Users.AddRange(InitialData.Users);
                db.OperationTokens.AddRange(InitialData.OperationTokens);
                db.SaveChanges();

                var adminUser = db.Users.SingleOrDefault(u => u.Email == UserData.Emails.AdminEmail);

                if (adminUser != null)
                {
                    CurrentUserJwtToken = authService.Authenticate(adminUser).Value;
                }
            });
        }
    }
}
