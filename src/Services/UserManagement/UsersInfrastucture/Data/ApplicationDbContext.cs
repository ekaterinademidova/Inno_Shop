using System.Reflection;
using UsersApplication.Interfaces.Data;

namespace UsersInfrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public bool DisableDomainEvents { get; set; } = false;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<User> Users => Set<User>();
        public virtual DbSet<OperationToken> OperationTokens => Set<OperationToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
