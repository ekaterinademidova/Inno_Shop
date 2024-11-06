using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using UsersApplication.Interfaces.RepositoryContracts;
using UsersApplication.Users.Commands.UpdateUser;
using UsersApplication.Users.EventHandlers.Domain;
using UsersDomain.Models;

namespace UsersApplication.Tests.CommandHandlers.Users.UpdateUser
{
    public sealed class UpdateUserCommandTestFixture : CommandHandlerFixtureBase
    {        
        public UpdateUserHandler CommandHandler { get; }
        public UserUpdatedEventHandler EventHandler { get; }
        public User? CapturedUser { get; private set; }

        public Mock<IUserRepository> UserRepositoryMock { get; }
        public Mock<ILogger<UserUpdatedEventHandler>> LoggerMock { get; }
        public UpdateUserCommandTestFixture()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            LoggerMock = new Mock<ILogger<UserUpdatedEventHandler>>();

            UnitOfWorkMock.Setup(uow => uow.User).Returns(UserRepositoryMock.Object);

            CommandHandler = new UpdateUserHandler(UnitOfWorkMock.Object);
            EventHandler = new UserUpdatedEventHandler(LoggerMock.Object);
            CapturedUser = null;
        }

        public CommandHandlerFixtureBase UserRepositoryMock_Setup_GetAsync_Returns_Null()
        {
            UserRepositoryMock
                .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), null, false, default))
                .ReturnsAsync((User?)null);

            return this;
        }

        public CommandHandlerFixtureBase UserRepositoryMock_Setup_GetAsync_Returns_ExistingUser(User existingUser)
        {
            UserRepositoryMock
                .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), null, false, default))
                .ReturnsAsync(existingUser);

            return this;
        }

        public CommandHandlerFixtureBase UserRepositoryMock_Setup_Update_Sets_CapturedUser()
        {
            UserRepositoryMock
                .Setup(repo => repo.Update(It.IsAny<User>()))
                .Callback<User>((user) => CapturedUser = user);

            return this;
        }

        public CommandHandlerFixtureBase UserRepositoryMock_Verify_GetAsync(Times times)
        {
            UserRepositoryMock
                .Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), null, false, default), times);

            return this;
        }

        public CommandHandlerFixtureBase UserRepositoryMock_Verify_Update(Times times)
        {
            UserRepositoryMock
                .Verify(repo => repo.Update(It.IsAny<User>()), times);

            return this;
        }
    }
}