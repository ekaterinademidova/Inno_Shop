using FluentAssertions;
using Moq;
using UsersApplication.Exceptions;
using UsersApplication.Users.Commands.UpdateUser;
using UsersDomain.Events;
using UsersDomain.Models;

namespace UsersApplication.Tests.CommandHandlers.Users.UpdateUser
{
    public sealed class UpdateUserCommandHandlerTests
    {
        private readonly UpdateUserCommandTestFixture _fixture = new();

        public UpdateUserCommandHandlerTests()
        {
        }

        [Fact] 
        public async Task Should_Update_User_And_Trigger_UserUpdatedEvent() 
        {
            // Arrange
            var existingUser = _fixture.SetupExistingUser();
            var userDto = _fixture.SetupUserDtoWithId(existingUser.Id);
            var command = new UpdateUserCommand(userDto);

            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_ExistingUser(existingUser);
            _fixture.UserRepositoryMock_Setup_Update_Sets_CapturedUser();
            _fixture.UnitOfWorkMock_Setup_SaveAsync_Verifiable();

            // Act
            var result = await _fixture.CommandHandler.Handle(command, default);
            var domainEvent = _fixture.CapturedUser!.DomainEvents.OfType<UserUpdatedEvent>().First();
            await _fixture.EventHandler.Handle(domainEvent, default);

            // Assert
            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.UserRepositoryMock_Verify_Update(Times.Once());
            _fixture.UnitOfWorkMock_Verify_SaveAsync();

            _fixture.CapturedUser.DomainEvents.Should().ContainSingle(e => e is UserUpdatedEvent);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Not_Update_Non_Existing_User()
        {
            // Arrange
            var userDto = _fixture.SetupUserDtoWithId();
            var command = new UpdateUserCommand(userDto);

            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_Null();

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<UserNotFoundException>()
                .WithMessage($"Entity \"{nameof(User)}\" ({userDto.Id}) was not found.");

            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.UserRepositoryMock_Verify_Update(Times.Never());
            _fixture.UnitOfWorkMock_Verify_No_SaveAsync();

            _fixture.CapturedUser.Should().BeNull();
        }
    }
}
