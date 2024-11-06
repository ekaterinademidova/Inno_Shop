using FluentAssertions;
using Moq.Protected;
using Moq;
using System.Net.Http.Json;
using System.Net;
using UsersInfrastructure.HttpClients;
using UsersApplication.Dtos;

namespace UsersInfrastructure.Tests
{
    public class ProductsServiceClientTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly ProductsServiceClient _client;

        public ProductsServiceClientTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:5050")
            };
            _client = new ProductsServiceClient(httpClient);
        }

        [Fact]
        public async Task CreateProductAsync_Should_Return_ProductId_When_Request_Is_Successful()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productDto = new ProductDto { Id = productId};

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post && req.RequestUri == new Uri($"https://localhost:5050/products")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(productDto)
                });

            // Act
            var result = await _client.CreateProductAsync(productDto);

            // Assert
            result.Should().Be(productId);
        }

        [Fact]
        public async Task CreateProductAsync_Should_Throw_InvalidOperationException_When_Request_Fails()
        {
            // Arrange
            var productDto = new ProductDto();

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post && req.RequestUri == new Uri($"https://localhost:5050/products")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            // Act
            var act = async () => await _client.CreateProductAsync(productDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*Service error.*");
        }

        [Fact]
        public async Task UpdateProductAsync_Should_Return_True_When_Update_Is_Successful()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productDto = new ProductDto { Id = productId };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Put && req.RequestUri == new Uri($"https://localhost:5050/products")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(true)
                });

            // Act
            var result = await _client.UpdateProductAsync(productDto);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateProductAsync_Should_Return_False_When_Update_Fails()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productDto = new ProductDto { Id = productId };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Put && req.RequestUri == new Uri($"https://localhost:5050/products")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            // Act
            var act = async () => await _client.UpdateProductAsync(productDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*Service error.*");
        }

        [Fact]
        public async Task DeleteProductAsync_Should_Return_True_When_Deletion_Is_Successful()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete && req.RequestUri == new Uri($"https://localhost:5050/products/{productId}")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(true)
                });

            // Act
            var result = await _client.DeleteProductAsync(productId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteProductAsync_Should_Return_False_When_Deletion_Fails()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete && req.RequestUri == new Uri($"https://localhost:5050/products/{productId}")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            // Act
            var act = async () => await _client.DeleteProductAsync(productId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*Service error.*");
        }
    }
}
