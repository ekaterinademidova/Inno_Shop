using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using System.Net;
using UsersInfrastructure.HttpClients.Handler;

namespace UsersInfrastructure.Tests
{
    public class AuthorizationHeaderHandlerTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly AuthorizationHeaderHandler _authorizationHeaderHandler;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

        public AuthorizationHeaderHandlerTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            // Mock the inner handler to verify requests and responses
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            // Set up the AuthorizationHeaderHandler with the mock handler
            _authorizationHeaderHandler = new AuthorizationHeaderHandler(_httpContextAccessorMock.Object)
            {
                InnerHandler = _httpMessageHandlerMock.Object
            };
        }

        [Fact]
        public async Task SendAsync_ShouldAddAuthorizationHeader_WhenAuthorizationHeaderExists()
        {
            // Arrange
            const string token = "sample_token";
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = $"Bearer {token}";
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            // Mock the inner handler response
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            var client = new HttpClient(_authorizationHeaderHandler);

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com");
            var response = await client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            request.Headers.Authorization.Should().NotBeNull();
            request.Headers.Authorization!.Scheme.Should().Be("Bearer");
            request.Headers.Authorization.Parameter.Should().Be(token);

            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task SendAsync_ShouldNotAddAuthorizationHeader_WhenAuthorizationHeaderDoesNotExist()
        {
            // Arrange
            var httpContext = new DefaultHttpContext(); // No Authorization header
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            // Mock the inner handler response
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            var client = new HttpClient(_authorizationHeaderHandler);

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com");
            var response = await client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            request.Headers.Authorization.Should().BeNull(); // No Authorization header should be added

            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
