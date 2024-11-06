using FluentAssertions;
using Moq;
using UsersApplication.Exceptions;
using UsersApplication.Users.Queries.GetUserById;
using UsersDomain.Models;

namespace UsersApplication.Tests.QueryHandlers.Users.GetUserById
{
    public class GetUserByIdQueryHandlerTests
    {
        private readonly GetUserByIdQueryTestFixture _fixture = new();

        [Fact]
        public async Task Should_Get_Existing_User()
        {
            // Arrange
            var existingUser = _fixture.SetupExistingUser();
            var query = new GetUserByIdQuery(existingUser.Id.Value);

            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_ExistingUser(existingUser);

            // Act
            var result = await _fixture.QueryHandler.Handle(query, default);

            // Assert
            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());

            result.Should().NotBeNull();
            result.User.Id.Should().Be(existingUser.Id.Value);
        }

        [Fact]
        public async Task Should_Not_Get_Non_Existing_User()
        {
            // Arrange
            var invalidUserId = Guid.NewGuid();
            var query = new GetUserByIdQuery(invalidUserId);
            _fixture.UserRepositoryMock_Setup_GetAsync_Returns_Null();

            // Act
            Func<Task> action = () => _fixture.QueryHandler.Handle(query, default);

            // Assert
            await action.Should().ThrowAsync<UserNotFoundException>()
                .WithMessage($"Entity \"{nameof(User)}\" ({invalidUserId}) was not found.");

            _fixture.UserRepositoryMock_Verify_GetAsync(Times.Once());
        }
    }
}
