using FluentAssertions;
using Moq;
using UsersApplication.Exceptions;
using UsersApplication.Users.Commands.ConfirmUserEmail;
using UsersDomain.Enums;
using UsersDomain.Models;
using UsersDomain.ValueObjects;

namespace UsersApplication.Tests.CommandHandlers.Users.ConfirmUserEmail
{
    public sealed class ConfirmUserEmailCommandHandlerTests
    {
        private readonly ConfirmUserEmailCommandTestFixture _fixture = new();

        public ConfirmUserEmailCommandHandlerTests()
        {
        }

        [Fact] 
        public async Task Should_Confirm_User_Email() 
        {
            // Arrange
            var existingUser = _fixture.SetupExistingUser();
            var existingOperationToken = OperationToken.Create(existingUser.Id, Guid.NewGuid(), OperationType.EmailConfirmation);
            var command = new ConfirmUserEmailCommand(existingOperationToken.Code);

            _fixture.OperationTokenRepositoryMock_Setup_GetAsync_Returns_ExistingOperationToken(existingOperationToken);
            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_ExistingUser(existingUser);
            _fixture.UserRepositoryMock_Setup_Update_Sets_CapturedUser();
            _fixture.OperationTokenRepositoryMock_Setup_Remove_Sets_CapturedOperationToken();
            _fixture.UnitOfWorkMock_Setup_SaveAsync_Verifiable();

            // Act
            var result = await _fixture.CommandHandler.Handle(command, default);

            // Assert
            _fixture.OperationTokenRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.UserRepositoryMock_Verify_Update(Times.Once());
            _fixture.OperationTokenRepositoryMock_Verify_Remove(Times.Once());
            _fixture.UnitOfWorkMock_Verify_SaveAsync();

            _fixture.CapturedUser.Should().NotBeNull();
            _fixture.CapturedUser!.IsConfirmed.Should().BeTrue();

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Not_Confirm_User_Email_For_Non_Existing_Operation_Token()
        {
            // Arrange
            var invalidTokenCode = Guid.NewGuid();
            var command = new ConfirmUserEmailCommand(invalidTokenCode);

            _fixture.OperationTokenRepositoryMock_Setup_GetAsync_Returns_Null();

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<OperationTokenNotFoundException>()
                .WithMessage($"Entity \"{nameof(OperationToken)} [{OperationType.EmailConfirmation}]\" ({invalidTokenCode}) was not found.");

            _fixture.OperationTokenRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Never());
            _fixture.UserRepositoryMock_Verify_Update(Times.Never());
            _fixture.OperationTokenRepositoryMock_Verify_Remove(Times.Never());
            _fixture.UnitOfWorkMock_Verify_No_SaveAsync();
        }

        [Fact]
        public async Task Should_Not_Confirm_User_Email_For_Invalid_Operation_Token()
        {
            // Arrange
            var existingOperationToken = OperationToken.Create(UserId.Of(Guid.NewGuid()), Guid.NewGuid(), OperationType.EmailConfirmation);
            existingOperationToken.SetExpiration(DateTime.UtcNow.AddDays(-3));
            var command = new ConfirmUserEmailCommand(existingOperationToken.Code);

            _fixture.OperationTokenRepositoryMock_Setup_GetAsync_Returns_ExistingOperationToken(existingOperationToken);
            _fixture.OperationTokenRepositoryMock_Setup_Remove_Sets_CapturedOperationToken();
            _fixture.UnitOfWorkMock_Setup_SaveAsync_Verifiable();

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<OperationTokenInvalidDataException>()
                .WithMessage($"{nameof(OperationToken)} [{OperationType.EmailConfirmation}] is invalid. The validity has expired.");

            _fixture.OperationTokenRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Never());
            _fixture.UserRepositoryMock_Verify_Update(Times.Never());
            _fixture.OperationTokenRepositoryMock_Verify_Remove(Times.Once());
            _fixture.UnitOfWorkMock_Verify_SaveAsync();
        }

        [Fact]
        public async Task Should_Not_Confirm_User_Email_For_Non_Existing_User()
        {
            // Arrange
            var existingOperationToken = OperationToken.Create(UserId.Of(Guid.NewGuid()), Guid.NewGuid(), OperationType.EmailConfirmation);
            var command = new ConfirmUserEmailCommand(existingOperationToken.Code);

            _fixture.OperationTokenRepositoryMock_Setup_GetAsync_Returns_ExistingOperationToken(existingOperationToken);
            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_Null();

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<UserNotFoundException>()
                .WithMessage($"Entity \"{nameof(User)}\" ({existingOperationToken.UserId.Value}) was not found.");

            _fixture.OperationTokenRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.UserRepositoryMock_Verify_Update(Times.Never());
            _fixture.OperationTokenRepositoryMock_Verify_Remove(Times.Never());
            _fixture.UnitOfWorkMock_Verify_No_SaveAsync();
        }
    }
}
