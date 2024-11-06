using MockQueryable;
using Moq;
using UsersApplication.Interfaces.RepositoryContracts;
using UsersApplication.Users.Queries.GetUsers;
using UsersDomain.Models;

namespace UsersApplication.Tests.QueryHandlers.Users.GetUsers
{
    public sealed class GetUsersQueryTestFixture : QueryHandlerFixtureBase
    {
        public Mock<IUserRepository> UserRepositoryMock { get; }
        public GetUsersQueryHandler QueryHandler { get; }

        public GetUsersQueryTestFixture()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            UnitOfWorkMock.Setup(uow => uow.User).Returns(UserRepositoryMock.Object);
            QueryHandler = new GetUsersQueryHandler(UnitOfWorkMock.Object);
        }

        public void UserRepositoryMock_Setup_GetAll_Returns_UsersList(List<User> users)
        {
            var usersMock = users.AsQueryable().BuildMock();
            UserRepositoryMock
                .Setup(repo => repo.GetAll(null, null))
                .Returns(usersMock);
        }

        public void UserRepositoryMock_Setup_GetTotalCountAsync_Returns_TotalCount(List<User> users)
        {
            UserRepositoryMock
                .Setup(repo => repo.GetTotalCountAsync(default))
                .ReturnsAsync(users.Count);
        }

        public void UserRepositoryMock_Verify_GetAll(Times times)
        {
            UserRepositoryMock
                .Verify(repo => repo.GetAll(null, null), times);
        }

        public void UserRepositoryMock_Verify_GetTotalCountAsync(Times times)
        {
            UserRepositoryMock
                .Verify(repo => repo.GetTotalCountAsync(default), times);
        }
    }
}