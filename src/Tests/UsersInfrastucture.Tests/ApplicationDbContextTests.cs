using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UsersDomain.Enums;
using UsersDomain.Models;
using UsersDomain.ValueObjects;
using UsersInfrastructure.Data;
using UsersInfrastructure.Data.Extensions;
using UsersInfrastructure.Tests.Fixtures;

namespace UsersInfrastructure.Tests
{
    public class ApplicationDbContextTests : IClassFixture<ApplicationDbContextFixture>
    {
        private readonly ApplicationDbContextFixture _fixture = new();
        private int n = 0;

        [Fact]
        public async Task Database_Should_MigrateAndCreateTables()
        {
            // Arrange & Act
            await _fixture.Context!.Database.MigrateAsync();
            var canConnect = await _fixture.Context!.Database.CanConnectAsync();

            // Assert
            canConnect.Should().BeTrue();
            var appliedMigrations = await _fixture.Context.Database.GetAppliedMigrationsAsync();
            appliedMigrations.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Can_Add_And_Delete_User()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = _fixture.SetupUser(email: $"simple{++n}@example.com");

            // Act - Add user
            await _fixture.Context!.Users.AddAsync(user);
            await _fixture.Context.SaveChangesAsync();

            var retrievedUser = await _fixture.Context.Users.FindAsync(user.Id);

            // Assert - Add user
            retrievedUser.Should().NotBeNull();

            // Act - Delete user
            _fixture.Context.Users.Remove(retrievedUser!);
            await _fixture.Context.SaveChangesAsync();

            // Assert - Delete user
            var deletedUser = await _fixture.Context.Users.FindAsync(user.Id);
            deletedUser.Should().BeNull();
        }

        [Fact]
        public async Task Can_Update_User_Details()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = _fixture.SetupUser(email: $"simple{++n}@example.com");

            // Act - Add user
            await _fixture.Context!.Users.AddAsync(user);
            await _fixture.Context.SaveChangesAsync();
            
            var retrievedUser = await _fixture.Context.Users.FindAsync(user.Id);

            // Assert - Add user
            retrievedUser.Should().NotBeNull();

            // Act - Update user details
            retrievedUser!.SetFirstName("UpdatedFirstName");
            retrievedUser.SetLastName("UpdatedLastName");
            retrievedUser.SetEmail("updated@example.com");
            retrievedUser.SetPassword("updatedPassword");
            retrievedUser.SetRole(UserRole.Admin);

            await _fixture.Context.SaveChangesAsync();

            // Assert - Update user details
            var updatedUser = await _fixture.Context.Users.FindAsync(user.Id);
            updatedUser.Should().NotBeNull();
            updatedUser!.FirstName.Should().Be("UpdatedFirstName");
            updatedUser.LastName.Should().Be("UpdatedLastName");
            updatedUser.Email.Should().Be("updated@example.com");
            updatedUser.Password.Should().Be("updatedPassword");
            updatedUser.Role.Should().Be(UserRole.Admin);
        }

        [Fact]
        public async Task User_Email_Should_Be_Unique()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = _fixture.SetupUser(email: "duplicated@example.com");

            await _fixture.Context!.Users.AddAsync(user);
            await _fixture.Context.SaveChangesAsync();

            var duplicatedUser =  _fixture.SetupUser(email: user.Email);

            // Act
            Func<Task> action = async () =>
            {
                await _fixture.Context.Users.AddAsync(duplicatedUser);
                await _fixture.Context.SaveChangesAsync();
            };

            // Assert
            await action.Should().ThrowAsync<DbUpdateException>();
        }

        [Fact]
        public async Task Can_Add_And_Delete_OperationToken()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = _fixture.SetupUser(email: $"simple{++n}@example.com");
            var token = _fixture.SetupOperationToken(userId: user.Id);

            // Act - Add user & token
            await _fixture.Context!.Users.AddAsync(user);
            await _fixture.Context!.OperationTokens.AddAsync(token);
            await _fixture.Context.SaveChangesAsync();

            var retrievedToken = await _fixture.Context.OperationTokens.FindAsync(token.Id);

            // Assert - Add token
            retrievedToken.Should().NotBeNull();

            // Act - Delete token
            _fixture.Context.OperationTokens.Remove(retrievedToken!);
            await _fixture.Context.SaveChangesAsync();

            // Assert - Delete token
            var deletedToken = await _fixture.Context.OperationTokens.FindAsync(token.Id);
            deletedToken.Should().BeNull();
        }

        [Fact]
        public async Task Can_Update_OperationToken_Details()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user1 = _fixture.SetupUser(email: $"simple{++n}@example.com");
            var user2 = _fixture.SetupUser(email: $"simple{++n}@example.com");
            var token = _fixture.SetupOperationToken(userId: user1.Id);

            // Act - Add users & token
            await _fixture.Context!.Users.AddAsync(user1);
            await _fixture.Context!.Users.AddAsync(user2);
            await _fixture.Context!.OperationTokens.AddAsync(token);
            await _fixture.Context.SaveChangesAsync();

            var retrievedToken = await _fixture.Context.OperationTokens.FindAsync(token.Id);

            // Assert - Add token
            retrievedToken.Should().NotBeNull();

            // Act - Update token details
            retrievedToken!.SetUserId(user2.Id);
            var newCode = Guid.NewGuid();
            retrievedToken.SetCode(newCode);
            retrievedToken.SetOperationType(OperationType.PasswordReset);
            var newExpiration = DateTime.UtcNow.AddMinutes(90);
            retrievedToken.SetExpiration(newExpiration);

            await _fixture.Context.SaveChangesAsync();

            // Assert - Update token details
            var updatedToken = await _fixture.Context.OperationTokens.FindAsync(token.Id);
            updatedToken.Should().NotBeNull();
            updatedToken!.UserId.Should().Be(user2.Id);
            updatedToken.Code.Should().Be(newCode);
            updatedToken.OperationType.Should().Be(OperationType.PasswordReset);
            updatedToken.Expiration.Should().Be(newExpiration);
        }

        [Fact]
        public async Task OperationToken_Code_Should_Be_Unique()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = _fixture.SetupUser(email: $"simple{++n}@example.com");
            var token = _fixture.SetupOperationToken(userId: user.Id);

            await _fixture.Context!.Users.AddAsync(user);
            await _fixture.Context!.OperationTokens.AddAsync(token);
            await _fixture.Context.SaveChangesAsync();

            var duplicateToken = _fixture.SetupOperationToken(userId: user.Id, code: token.Code);

            // Act
            Func<Task> action = async () =>
            {
                await _fixture.Context.OperationTokens.AddAsync(duplicateToken);
                await _fixture.Context.SaveChangesAsync();
            };

            // Assert
            await action.Should().ThrowAsync<DbUpdateException>();
        }
    }
}
