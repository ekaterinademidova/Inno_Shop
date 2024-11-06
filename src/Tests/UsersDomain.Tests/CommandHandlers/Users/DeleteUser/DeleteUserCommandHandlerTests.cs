using FluentAssertions;
using Moq;
using UsersApplication.Exceptions;
using UsersApplication.Users.Commands.DeleteUser;
using UsersDomain.Models;

namespace UsersApplication.Tests.CommandHandlers.Users.DeleteUser
{
    public sealed class DeleteUserCommandHandlerTests
    {
        private readonly DeleteUserCommandTestFixture _fixture = new();

        public DeleteUserCommandHandlerTests()
        {
        }

        [Fact] 
        public async Task Should_Delete_User() 
        {
            // Arrange
            var existingUser = _fixture.SetupExistingUser();
            var userDto = _fixture.SetupUserDtoWithId(existingUser.Id);
            var command = new DeleteUserCommand(userDto.Id);

            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_ExistingUser(existingUser);
            _fixture.UserRepositoryMock_Setup_Remove_Sets_CapturedUser();
            _fixture.UnitOfWorkMock_Setup_SaveAsync_Verifiable();

            // Act
            var result = await _fixture.CommandHandler.Handle(command, default);

            // Assert
            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.UserRepositoryMock_Verify_Remove(Times.Once());
            _fixture.UnitOfWorkMock_Verify_SaveAsync();

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Not_Delete_Non_Existing_User()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeleteUserCommand(userId);

            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_Null();

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<UserNotFoundException>()
                .WithMessage($"Entity \"{nameof(User)}\" ({userId}) was not found.");

            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.UserRepositoryMock_Verify_Remove(Times.Never());
            _fixture.UnitOfWorkMock_Verify_No_SaveAsync();

            _fixture.CapturedUser.Should().BeNull();
        }
    }
}
