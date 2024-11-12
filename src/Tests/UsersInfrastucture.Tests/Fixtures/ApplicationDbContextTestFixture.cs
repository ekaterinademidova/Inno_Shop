using Microsoft.EntityFrameworkCore;
using UsersDomain.Enums;
using UsersDomain.Models;
using UsersDomain.ValueObjects;
using UsersInfrastructure.Data;

namespace UsersInfrastructure.Tests.Fixtures
{
    public class ApplicationDbContextFixture : IDisposable
    {
        //private readonly string  _connectionString = "Server=localhost;Database=UsersDbTest;User Id=sa;Password=SwN12345678;Encrypt=False;TrustServerCertificate=True";
        private readonly DbContextOptions<ApplicationDbContext> _options;
        public ApplicationDbContext? Context { get; set; }

        public ApplicationDbContextFixture()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                //.UseSqlServer(_connectionString)
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            Context = new ApplicationDbContext(_options);
            Context.Database.EnsureCreated();
        }

        //public void ClearDatabase()
        //{
        //    if (Context is not null)
        //    {
        //        Context.Database.EnsureDeleted();
        //        Context.Database.EnsureCreated();
        //    }
        //}

        public void Dispose()
        {
            if (Context is not null)
            {
                Context.Database.EnsureDeleted();
                Context.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public User SetupUser(
            UserId? id = null,
            string? email = null,
            string? password = null) =>
        new()
        {
            Id = id ?? UserId.Of(Guid.NewGuid()),
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Email = email ?? "test.email@gmail.com",
            Password = password ?? "Po=PF]PC6t.?8?ks)A6W",
        };

        public OperationToken SetupOperationToken(
            UserId? userId = null,
            Guid? code = null,
            OperationType? operationType = null) =>
        new()
        {
            Id = OperationTokenId.Of(Guid.NewGuid()),
            UserId = userId ?? UserId.Of(Guid.NewGuid()),
            OperationType = operationType ?? OperationType.EmailConfirmation,
            Code = code ?? Guid.NewGuid(),
            Expiration = DateTime.UtcNow.AddMinutes(60)
        };
    }
}
