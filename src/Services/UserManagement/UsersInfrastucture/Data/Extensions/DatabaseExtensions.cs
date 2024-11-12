using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace UsersInfrastructure.Data.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.MigrateAsync().GetAwaiter().GetResult();

            context.DisableDomainEvents = true;
            try
            {
                await SeedAsync(context);
            }
            finally
            {
                context.DisableDomainEvents = false;
            }
}

        private static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedUsersAsync(context);
            await SeedOperationTokensAsync(context);
        }

        private static async Task SeedUsersAsync(ApplicationDbContext context)
        {
            if (!await context.Users.AnyAsync())
            {
                await context.Users.AddRangeAsync(InitialData.Users);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedOperationTokensAsync(ApplicationDbContext context)
        {
            if (!await context.OperationTokens.AnyAsync())
            {
                await context.OperationTokens.AddRangeAsync(InitialData.OperationTokens);
                await context.SaveChangesAsync();
            }
        }
    }
}
