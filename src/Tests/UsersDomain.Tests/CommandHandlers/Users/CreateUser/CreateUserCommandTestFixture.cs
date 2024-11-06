using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using UsersApplication.Interfaces.RepositoryContracts;
using UsersApplication.Interfaces.ServiceContracts;
using UsersApplication.Users.Commands.CreateUser;
using UsersApplication.Users.EventHandlers.Domain;
using UsersDomain.Models;

namespace UsersApplication.Tests.CommandHandlers.Users.CreateUser
{

    public sealed class CreateUserCommandTestFixture : CommandHandlerFixtureBase
    {
        public CreateUserHandler CommandHandler { get; }
        public UserCreatedEventHandler EventHandler { get; }
        public User? CapturedUser { get; private set; }

        public Mock<IUserRepository> UserRepositoryMock { get; }
        public Mock<IEmailService> EmailServiceMock { get; }
        public Mock<ILogger<UserCreatedEventHandler>> LoggerMock { get; }

        public CreateUserCommandTestFixture()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            EmailServiceMock = new Mock<IEmailService>();
            LoggerMock = new Mock<ILogger<UserCreatedEventHandler>>();

            UnitOfWorkMock.Setup(uow => uow.User).Returns(UserRepositoryMock.Object);

            CommandHandler = new CreateUserHandler(UnitOfWorkMock.Object);
            EventHandler = new UserCreatedEventHandler(EmailServiceMock.Object, LoggerMock.Object);
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

        public CommandHandlerFixtureBase UserRepositoryMock_Setup_AddAsync_Sets_CapturedUser()
        {
            UserRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<User>(), default))
                .Callback<User, CancellationToken>((user, ct) => CapturedUser = user);

            return this;
        }

        public CommandHandlerFixtureBase EmailServiceMock_Setup_SendEmailConfirmationAsync()
        {
            EmailServiceMock
                .Setup(sender => sender.SendEmailConfirmationAsync(It.IsAny<User>(), default))
                .Returns(Task.CompletedTask)
                .Verifiable();

            return this;
        }

        public CommandHandlerFixtureBase UserRepositoryMock_Verify_GetAsync(Times times)
        {
            UserRepositoryMock
                .Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), null, false, default), times);

            return this;
        }

        public CommandHandlerFixtureBase UserRepositoryMock_Verify_AddAsync(Times times)
        {
            UserRepositoryMock
                .Verify(repo => repo.AddAsync(It.IsAny<User>(), default), times);

            return this;
        }

        public CommandHandlerFixtureBase EmailServiceMock_Verify_SendEmailConfirmationAsync(Times times)
        {
            EmailServiceMock
                .Verify(sender => sender.SendEmailConfirmationAsync(It.IsAny<User>(), default), times);

            return this;
        }
    }
}