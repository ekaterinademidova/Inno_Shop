using Moq;
using System.Linq.Expressions;
using UsersApplication.Interfaces.RepositoryContracts;
using UsersApplication.Users.Commands.ResetPassword;
using UsersDomain.Models;

namespace UsersApplication.Tests.CommandHandlers.Users.ResetPassword
{

    public sealed class ResetPasswordCommandTestFixture : CommandHandlerFixtureBase
    {        
        public ResetPasswordHandler CommandHandler { get; }
        //public UserUpdatedEventHandler EventHandler { get; }
        public User? CapturedUser { get; private set; }
        public OperationToken? CapturedOperationToken { get; private set; }

        public Mock<IUserRepository> UserRepositoryMock { get; }
        public Mock<IOperationTokenRepository> OperationTokenRepositoryMock { get; }
        //public Mock<ILogger<UserUpdatedEventHandler>> LoggerMock { get; }
        public ResetPasswordCommandTestFixture()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            OperationTokenRepositoryMock = new Mock<IOperationTokenRepository>();
            //LoggerMock = new Mock<ILogger<UserUpdatedEventHandler>>();

            UnitOfWorkMock.Setup(uow => uow.User).Returns(UserRepositoryMock.Object);
            UnitOfWorkMock.Setup(uow => uow.OperationToken).Returns(OperationTokenRepositoryMock.Object);

            CommandHandler = new ResetPasswordHandler(UnitOfWorkMock.Object);
            //EventHandler = new UserUpdatedEventHandler(LoggerMock.Object);
            CapturedUser = null;
            CapturedOperationToken = null;
        }

        public CommandHandlerFixtureBase OperationTokenRepositoryMock_Setup_GetAsync_Returns_Null()
        {
            OperationTokenRepositoryMock
                .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<OperationToken, bool>>>(), null, false, default))
                .ReturnsAsync((OperationToken?)null);

            return this;
        }

        public CommandHandlerFixtureBase OperationTokenRepositoryMock_Setup_GetAsync_Returns_ExistingOperationToken(OperationToken operationToken)
        {
            OperationTokenRepositoryMock
                .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<OperationToken, bool>>>(), null, false, default))
                .ReturnsAsync(operationToken);

            return this;
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

        public CommandHandlerFixtureBase OperationTokenRepositoryMock_Setup_Remove_Sets_CapturedOperationToken()
        {
            OperationTokenRepositoryMock
                .Setup(repo => repo.Update(It.IsAny<OperationToken>()))
                .Callback<OperationToken>((operationToken) => CapturedOperationToken = operationToken);

            return this;
        }

        public CommandHandlerFixtureBase UserRepositoryMock_Verify_GetAsync(Times times)
        {
            UserRepositoryMock
                .Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), null, false, default), times);

            return this;
        }

        public CommandHandlerFixtureBase OperationTokenRepositoryMock_Verify_GetAsync(Times times)
        {
            OperationTokenRepositoryMock
                .Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<OperationToken, bool>>>(), null, false, default), times);

            return this;
        }

        public CommandHandlerFixtureBase UserRepositoryMock_Verify_Update(Times times)
        {
            UserRepositoryMock
                .Verify(repo => repo.Update(It.IsAny<User>()), times);

            return this;
        }

        public CommandHandlerFixtureBase OperationTokenRepositoryMock_Verify_Remove(Times times)
        {
            OperationTokenRepositoryMock
                .Verify(repo => repo.Remove(It.IsAny<OperationToken>()), times);

            return this;
        }
    }
}