using Moq;
using System.Linq.Expressions;
using UsersApplication.Interfaces.RepositoryContracts;
using UsersApplication.Interfaces.ServiceContracts;
using UsersApplication.Users.Commands.LoginUser;
using UsersApplication.ValueObjects;
using UsersDomain.Models;

namespace UsersApplication.Tests.CommandHandlers.Users.LoginUser
{

    public sealed class LoginUserCommandTestFixture : CommandHandlerFixtureBase
    {
        public LoginUserHandler CommandHandler { get; }

        public Mock<IUserRepository> UserRepositoryMock { get; }
        public Mock<IAuthenticationService> AuthenticationServiceMock { get; }

        public LoginUserCommandTestFixture()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            AuthenticationServiceMock = new Mock<IAuthenticationService>();

            UnitOfWorkMock.Setup(uow => uow.User).Returns(UserRepositoryMock.Object);

            CommandHandler = new LoginUserHandler(UnitOfWorkMock.Object, AuthenticationServiceMock.Object);
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

        public CommandHandlerFixtureBase AuthenticationServiceMock_Setup_Authenticate(JwtToken jwtToken)
        {
            AuthenticationServiceMock
                .Setup(auth => auth.Authenticate(It.IsAny<User>()))
                .Returns(jwtToken)
                .Verifiable();

            return this;
        }

        public CommandHandlerFixtureBase UserRepositoryMock_Verify_GetAsync(Times times)
        {
            UserRepositoryMock
                .Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), null, false, default), times);

            return this;
        }

        public CommandHandlerFixtureBase AuthenticationService_Verify_Authenticate(Times times)
        {
            AuthenticationServiceMock
                .Verify(auth => auth.Authenticate(It.IsAny<User>()), times);

            return this;
        }
    }
}