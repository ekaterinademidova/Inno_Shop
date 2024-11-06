using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using UsersApplication.Interfaces.RepositoryContracts;
using UsersApplication.Interfaces.ServiceContracts;
using UsersApplication.Users.Commands.CreateUser;
using UsersApplication.Users.Commands.SeekPasswordReset;
using UsersApplication.Users.EventHandlers.Domain;
using UsersDomain.Models;

namespace UsersApplication.Tests.CommandHandlers.Users.SeekPasswordReset
{

    public sealed class SeekPasswordResetCommandTestFixture : CommandHandlerFixtureBase
    {
        public SeekPasswordResetHandler CommandHandler { get; }
        public User? CapturedUser { get; private set; }

        public Mock<IUserRepository> UserRepositoryMock { get; }
        public Mock<IEmailService> EmailServiceMock { get; }

        public SeekPasswordResetCommandTestFixture()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            EmailServiceMock = new Mock<IEmailService>();

            UnitOfWorkMock.Setup(uow => uow.User).Returns(UserRepositoryMock.Object);

            CommandHandler = new SeekPasswordResetHandler(UnitOfWorkMock.Object, EmailServiceMock.Object);
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

        public CommandHandlerFixtureBase EmailServiceMock_Setup_SendPasswordResetAsync()
        {
            EmailServiceMock
                .Setup(sender => sender.SendPasswordResetAsync(It.IsAny<User>(), default))
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

        public CommandHandlerFixtureBase EmailServiceMock_Verify_SendPasswordResetAsync(Times times)
        {
            EmailServiceMock
                .Verify(sender => sender.SendPasswordResetAsync(It.IsAny<User>(), default), times);

            return this;
        }
    }
}