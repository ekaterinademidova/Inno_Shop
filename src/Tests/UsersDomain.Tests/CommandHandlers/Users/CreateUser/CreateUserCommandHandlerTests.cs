using FluentAssertions;
using Moq;
using UsersApplication.Exceptions;
using UsersApplication.Users.Commands.CreateUser;
using UsersDomain.Events;

namespace UsersApplication.Tests.CommandHandlers.Users.CreateUser
{
    public sealed class CreateUserCommandHandlerTests
    {
        private readonly CreateUserCommandTestFixture _fixture = new();

        public CreateUserCommandHandlerTests()
        {
        }

        [Fact] 
        public async Task Should_Create_User_And_Send_EmailConfirmation_Email() 
        {
            // Arrange
            var userDto = _fixture.SetupUserDto();
            var command = new CreateUserCommand(userDto);

            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_Null();
            _fixture.UserRepositoryMock_Setup_AddAsync_Sets_CapturedUser();
            _fixture.UnitOfWorkMock_Setup_SaveAsync_Verifiable();
            _fixture.EmailServiceMock_Setup_SendEmailConfirmationAsync();

            // Act
            var result = await _fixture.CommandHandler.Handle(command, default);
            var domainEvent = _fixture.CapturedUser!.DomainEvents.OfType<UserCreatedEvent>().First();
            await _fixture.EventHandler.Handle(domainEvent, default);

            // Assert
            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.UserRepositoryMock_Verify_AddAsync(Times.Once());            
            _fixture.UnitOfWorkMock_Verify_SaveAsync();
            _fixture.EmailServiceMock_Verify_SendEmailConfirmationAsync(Times.Once());

            _fixture.CapturedUser.DomainEvents.Should().ContainSingle(e => e is UserCreatedEvent);

            result.Should().NotBeNull();
            result.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task Should_Not_Create_User_For_Already_Existing_Email() 
        {
            // Arrange
            var userDto = _fixture.SetupUserDto();
            var existingUser = _fixture.SetupExistingUser(email: userDto.Email);            
            var command = new CreateUserCommand(userDto);

            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_ExistingUser(existingUser);

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<UserInvalidDataException>()
                .WithMessage($"The user with email \"{userDto.Email})\" already exists.");

            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
            _fixture.UserRepositoryMock_Verify_AddAsync(Times.Never());
            _fixture.UnitOfWorkMock_Verify_No_SaveAsync();
        }
    }
}
