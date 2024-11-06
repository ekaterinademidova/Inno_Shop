using BuildingBlocks.Pagination;
using FluentAssertions;
using MockQueryable;
using Moq;
using UsersApplication.Users.Queries.GetUsers;

namespace UsersApplication.Tests.QueryHandlers.Users.GetUsers
{
    public class GetUsersQueryHandlerTests
    {
        private readonly GetUsersQueryTestFixture _fixture = new();

        [Fact]
        public async Task Should_Return_Ordered_And_Paginated_Users()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 2;
            var users = _fixture.SetupExistingUsersList();            
            var query = new GetUsersQuery(new PaginationRequest(pageIndex, pageSize));

            _fixture.UserRepositoryMock_Setup_GetAll_Returns_UsersList(users);
            _fixture.UserRepositoryMock_Setup_GetTotalCountAsync_Returns_TotalCount(users);

            // Act
            var result = await _fixture.QueryHandler.Handle(query, default);

            // Assert
            _fixture.UserRepositoryMock_Verify_GetAll(Times.Once());
            _fixture.UserRepositoryMock_Verify_GetTotalCountAsync(Times.Once());

            result.Should().NotBeNull();            
            result.Users.PageIndex.Should().Be(pageIndex);
            result.Users.PageSize.Should().Be(pageSize);
            result.Users.Count.Should().Be(users.Count);
            result.Users.Data.Should().HaveCountLessThanOrEqualTo(pageSize);
            result.Users.Data.First().LastName.Should().Be("CTestLastName");
            result.Users.Data.First().FirstName.Should().Be("ATestFirstName");
        }

        [Fact]
        public async Task Should_Handle_Empty_Users_List()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 2;
            var query = new GetUsersQuery(new PaginationRequest(pageIndex, pageSize));

            _fixture.UserRepositoryMock_Setup_GetAll_Returns_UsersList([]);
            _fixture.UserRepositoryMock_Setup_GetTotalCountAsync_Returns_TotalCount([]);

            // Act
            var result = await _fixture.QueryHandler.Handle(query, default);

            // Assert
            _fixture.UserRepositoryMock_Verify_GetAll(Times.Once());
            _fixture.UserRepositoryMock_Verify_GetTotalCountAsync(Times.Once());

            result.Should().NotBeNull();
            result.Users.PageIndex.Should().Be(pageIndex);
            result.Users.PageSize.Should().Be(pageSize);
            result.Users.Count.Should().Be(0);
            result.Users.Data.Should().BeEmpty();
        }
    }
}
