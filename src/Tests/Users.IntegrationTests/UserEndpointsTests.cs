using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UsersAPI.Endpoints.Users;
using UsersApplication.Dtos;
using UsersDomain.Constants;
using UsersDomain.Enums;
using UsersDomain.Models;
using UsersDomain.ValueObjects;

namespace UsersAPI.IntegrationTests
{
    public class UserEndpointsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public UserEndpointsTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", factory.CurrentUserJwtToken);
        }

        #region CreateUser
        [Fact]
        public async Task CreateUser_WithValidData_ReturnsCreatedResponse()
        {
            // Arrange
            var request = new CreateUserRequest(SetupUserDto(email: "test0.email@gmail.com"));

            // Act
            var response = await _client.PostAsJsonAsync("/users", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var result = await response.Content.ReadFromJsonAsync<CreateUserResponse>();
            result.Should().NotBeNull();
            result!.Id.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateUser_WithDuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateUserRequest(SetupUserDto(email: "test1.email@gmail.com"));
            await _client.PostAsJsonAsync("/users", request); 

            // Act
            var response = await _client.PostAsJsonAsync("/users", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            problemDetails.Should().NotBeNull();
            problemDetails!.Title.Should().Be("UserInvalidDataException");
            problemDetails.Detail.Should().Be($"The user with email \"{request.User.Email})\" already exists.");
        }
        #endregion

        #region UpdateUser
        [Fact]
        public async Task UpdateUser_WithValidData_ReturnsOkResponse()
        {
            // Arrange
            var request = new UpdateUserRequest(SetupUserDtoWithId(UserData.Ids.User1Id));

            // Act
            var response = await _client.PutAsJsonAsync($"/users", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<UpdateUserResponse>();
            result.Should().NotBeNull();
            result!.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateUser_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var request = new UpdateUserRequest(SetupUserDtoWithId());

            // Act
            var response = await _client.PutAsJsonAsync($"/users", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        #endregion

        #region DeleteUser
        [Fact]
        public async Task DeleteUser_WithValidId_ReturnsOkResponse()
        {
            // Arrange & Act
            var response = await _client.DeleteAsync($"/users/{UserData.Ids.User3Id.Value}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<DeleteUserResponse>();
            result.Should().NotBeNull();
            result!.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUser_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var nonExistentUserId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"/users/{nonExistentUserId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        #endregion

        #region ConfirmUserEmail
        [Fact]
        public async Task ConfirmUserEmail_WithValidToken_ReturnsOkResponse()
        {
            // Arrange
            var token = OperationTokenData.Codes.User1EmailConfirmationCode;

            // Act
            var response = await _client.PostAsync($"/users/confirm-email?token={token}", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ConfirmUserEmailResponse>();
            result.Should().NotBeNull();
            result!.IsSuccess.Should().BeTrue();
        }


        [Fact]
        public async Task ConfirmUserEmail_WithInvalidToken_ReturnsBadRequest()
        {
            // Arrange
            var token = Guid.NewGuid();

            // Act
            var response = await _client.PostAsync($"/users/confirm-email?token={token}", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            problemDetails.Should().NotBeNull();
            problemDetails!.Title.Should().Be("OperationTokenNotFoundException");
            problemDetails.Detail.Should().Be($"Entity \"{nameof(OperationToken)} [{OperationType.EmailConfirmation}]\" ({token}) was not found.");
        }
        #endregion

        #region LoginUser
        [Fact]
        public async Task LoginUser_WithValidCredentials_ReturnsOkResponse()
        {
            // Arrange
            var request = new LoginUserRequest(UserData.Emails.User2Email, UserData.Passwords.User2Password);

            // Act
            var response = await _client.PostAsJsonAsync("/users/login", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<LoginUserResponse>();
            result.Should().NotBeNull();
            result!.Token.Should().NotBeNull();
        }

        [Fact]
        public async Task LoginUser_WithInvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginUserRequest("test.email@gmail.com", "wrongPassword");

            // Act
            var response = await _client.PostAsJsonAsync("/users/login", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            problemDetails.Should().NotBeNull();
            problemDetails!.Title.Should().Be("UserNotFoundException");
            problemDetails.Detail.Should().Be($"Entity \"{nameof(User)}\" ({request.Email}) was not found.");
        }
        #endregion

        #region SeekPasswordReset
        [Fact]
        public async Task SeekPasswordReset_WithValidEmail_ReturnsOkResponse()
        {
            // Arrange
            var request = new SeekPasswordResetRequest(UserData.Emails.AdminEmail);

            // Act
            var response = await _client.PostAsJsonAsync("/users/seek-password-reset", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<SeekPasswordResetResponse>();
            result.Should().NotBeNull();
            result!.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task SeekPasswordReset_WithNonExistentEmail_ReturnsNotFound()
        {
            // Arrange
            var request = new SeekPasswordResetRequest("nonexistent.email@gmail.com");

            // Act
            var response = await _client.PostAsJsonAsync("/users/seek-password-reset", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        #endregion

        #region ResetPassword
        [Fact]
        public async Task ResetPassword_WithValidTokenAndPassword_ReturnsOkResponse()
        {
            // Arrange
            var token = OperationTokenData.Codes.User1PasswordResetCode;
            var password = new PasswordParam("NewSecurePassword!123");

            // Act
            var response = await _client.PostAsJsonAsync($"/users/reset-password?token={token}", password);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ConfirmUserEmailResponse>();
            result.Should().NotBeNull();
            result!.IsSuccess.Should().BeTrue();
        }


        [Fact]
        public async Task ResetPassword_WithInvalidToken_ReturnsBadRequest()
        {
            // Arrange
            var token = Guid.NewGuid();
            var password = new PasswordParam("NewSecurePassword!123");

            // Act
            var response = await _client.PostAsJsonAsync($"/users/reset-password?token={token}", password);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            problemDetails.Should().NotBeNull();
            problemDetails!.Title.Should().Be("OperationTokenNotFoundException");
            problemDetails.Detail.Should().Be($"Entity \"{nameof(OperationToken)} [{OperationType.PasswordReset}]\" ({token}) was not found.");
        }
        #endregion

        public UserDto SetupUserDto(
            string? email = null) =>
        new()
        {
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Email = email ?? "test.email@gmail.com",
            Password = "testPassword111"
        };

        public UserDto SetupUserDtoWithId(
             UserId? id = null) =>
        new()
        {
            Id = id is null ? Guid.NewGuid() : id.Value,
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Email = "test.email@gmail.com",
            Password = "Po=PF]PC6t.?8?ks)A6W"
        };
    }
}
