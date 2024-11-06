using FluentAssertions;
using Moq;
using UsersApplication.Exceptions;
using UsersApplication.Users.Commands.SeekPasswordReset;
using UsersDomain.Models;

namespace UsersApplication.Tests.CommandHandlers.Users.SeekPasswordReset
{
    public sealed class SeekPasswordResetCommandHandlerTests
    {
        private readonly SeekPasswordResetCommandTestFixture _fixture = new();

        public SeekPasswordResetCommandHandlerTests()
        {
        }

        [Fact] 
        public async Task Should_Seek_Password_Reset() 
        {
            // Arrange
            var existingUser = _fixture.SetupExistingUser();
            var command = new SeekPasswordResetCommand(existingUser.Email);

            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_ExistingUser(existingUser);
            _fixture.EmailServiceMock_Setup_SendPasswordResetAsync();

            // Act
            var result = await _fixture.CommandHandler.Handle(command, default);

            // Assert
            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.EmailServiceMock_Verify_SendPasswordResetAsync(Times.Once());

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Not_Seek_Password_Reset_For_Invalid_Email()
        {
            // Arrange
            var invalidEmail = "test.email@gmail.com";
            var command = new SeekPasswordResetCommand(invalidEmail);

            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_Null();

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<UserNotFoundException>()
                .WithMessage($"Entity \"{nameof(User)}\" ({invalidEmail}) was not found.");

            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.EmailServiceMock_Verify_SendPasswordResetAsync(Times.Never());
        }
    }
}
