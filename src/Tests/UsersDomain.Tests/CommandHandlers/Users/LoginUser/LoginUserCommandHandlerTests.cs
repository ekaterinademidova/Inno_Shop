using FluentAssertions;
using Moq;
using UsersApplication.Exceptions;
using UsersApplication.Users.Commands.LoginUser;
using UsersApplication.ValueObjects;
using UsersDomain.Models;

namespace UsersApplication.Tests.CommandHandlers.Users.LoginUser
{
    public sealed class LoginUserCommandHandlerTests
    {
        private readonly LoginUserCommandTestFixture _fixture = new();

        public LoginUserCommandHandlerTests()
        {
        }

        [Fact] 
        public async Task Should_Login_User_And_Return_Valid_JwtToken() 
        {
            // Arrange
            var existingUser = _fixture.SetupExistingUser();
            var jwtToken = new JwtToken { Value = "jwt-token-success" };
            var command = new LoginUserCommand(existingUser.Email, existingUser.Password);

            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_ExistingUser(existingUser);
            _fixture.AuthenticationServiceMock_Setup_Authenticate(jwtToken);

            // Act
            var result = await _fixture.CommandHandler.Handle(command, default);

            // Assert
            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.AuthenticationService_Verify_Authenticate(Times.Once());

            result.Should().NotBeNull();
            result.Token.Value.Should().Be(jwtToken.Value);
        }

        [Fact]
        public async Task Should_Not_Login_User_For_Invalid_Email()
        {
            // Arrange
            var invalidEmail = "test.email@gmail.com";
            var command = new LoginUserCommand(invalidEmail, "Po=PF]PC6t.?8?ks)A6W");

            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_Null();

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<UserNotFoundException>()
                .WithMessage($"Entity \"{nameof(User)}\" ({invalidEmail}) was not found.");

            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.AuthenticationService_Verify_Authenticate(Times.Never());
        }

        [Fact]
        public async Task Should_Not_Login_User_For_Invalid_Password()
        {
            // Arrange
            var existingUser = _fixture.SetupExistingUser();
            var invalidPassword = "invalidPassword";
            var command = new LoginUserCommand(existingUser.Email, invalidPassword);

            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_ExistingUser(existingUser);

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<UserInvalidDataException>()
                .WithMessage("Invalid password.");

            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.AuthenticationService_Verify_Authenticate(Times.Never());
        }
    }
}
