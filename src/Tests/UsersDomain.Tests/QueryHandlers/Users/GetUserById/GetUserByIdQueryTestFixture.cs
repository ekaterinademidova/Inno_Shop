using Moq;
using System.Linq.Expressions;
using UsersApplication.Interfaces.RepositoryContracts;
using UsersApplication.Users.Queries.GetUserById;
using UsersDomain.Models;

namespace UsersApplication.Tests.QueryHandlers.Users.GetUserById
{
    public sealed class GetUserByIdQueryTestFixture : QueryHandlerFixtureBase
    {
        public Mock<IUserRepository> UserRepositoryMock { get; }
        public GetUserByIdQueryHandler QueryHandler { get; }

        public GetUserByIdQueryTestFixture()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            UnitOfWorkMock.Setup(uow => uow.User).Returns(UserRepositoryMock.Object);
            QueryHandler = new GetUserByIdQueryHandler(UnitOfWorkMock.Object);
        }

        public QueryHandlerFixtureBase UserRepositoryMock_Setup_GetAsync_Returns_Null()
        {
            UserRepositoryMock
                .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), null, false, default))
                .ReturnsAsync((User?)null);

            return this;
        }

        public QueryHandlerFixtureBase UserRepositoryMock_Setup_GetAsync_Returns_ExistingUser(User existingUser)
        {
            UserRepositoryMock
                .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), null, false, default))
                .ReturnsAsync(existingUser);

            return this;
        }

        public QueryHandlerFixtureBase UserRepositoryMock_Verify_GetAsync(Times times)
        {
            UserRepositoryMock
                .Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), null, false, default), times);

            return this;
        }
    }
}