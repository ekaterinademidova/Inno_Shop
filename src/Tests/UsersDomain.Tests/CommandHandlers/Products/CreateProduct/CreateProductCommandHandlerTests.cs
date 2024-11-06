using FluentAssertions;
using Moq;
using UsersApplication.Products.Commands.CreateProduct;

namespace UsersApplication.Tests.CommandHandlers.Products.CreateProduct
{
    public sealed class CreateProductCommandHandlerTests
    {
        private readonly CreateProductCommandTestFixture _fixture = new();

        [Fact]
        public async Task Should_Create_Product()
        {
            // Arrange
            var productDto = _fixture.SetupProductDto();
            var command = new CreateProductCommand(productDto);

            _fixture.ProductsServiceClientMock_Setup_CreateProductAsync_Returns_ProductId(productDto);

            // Act
            var result = await _fixture.CommandHandler.Handle(command, default);

            // Assert
            _fixture.ProductsServiceClientMock_Verify_CreateProductAsync(Times.Once());

            result.Should().NotBeNull();
            result.ProductId.Should().NotBe(Guid.Empty);
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException), "Service error.")]
        public async Task Should_Throw_Exception_For_Service_Error(Type expectedExceptionType, string expectedErrorMessage)
        {
            // Arrange
            var productDto = _fixture.SetupProductDto();
            var command = new CreateProductCommand(productDto);

            _fixture.ProductsServiceClientMock_Setup_CreateProductAsync_Throw_Exception(expectedExceptionType, expectedErrorMessage);

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*{expectedErrorMessage}*");

            _fixture.ProductsServiceClientMock_Verify_CreateProductAsync(Times.Once());
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException), "Network error.")]
        public async Task Should_Throw_Exception_For_Network_Error(Type expectedExceptionType, string expectedErrorMessage)
        {
            // Arrange
            var productDto = _fixture.SetupProductDto();
            var command = new CreateProductCommand(productDto);

            _fixture.ProductsServiceClientMock_Setup_CreateProductAsync_Throw_Exception(expectedExceptionType, expectedErrorMessage);

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*{expectedErrorMessage}*");

            _fixture.ProductsServiceClientMock_Verify_CreateProductAsync(Times.Once());
        }

        [Theory]
        [InlineData(typeof(TimeoutException), "The request timed out.")]
        public async Task Should_Throw_Exception_For_Timeout_Error(Type expectedExceptionType, string expectedErrorMessage)
        {
            // Arrange
            var productDto = _fixture.SetupProductDto();
            var command = new CreateProductCommand(productDto);

            _fixture.ProductsServiceClientMock_Setup_CreateProductAsync_Throw_Exception(expectedExceptionType, expectedErrorMessage);

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<TimeoutException>()
                .WithMessage($"*{expectedErrorMessage}*");

            _fixture.ProductsServiceClientMock_Verify_CreateProductAsync(Times.Once());
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException), "Failed to deserialize the response or the response is null.")]
        public async Task Should_Throw_Exception_For_Json_Error(Type expectedExceptionType, string expectedErrorMessage)
        {
            // Arrange
            var productDto = _fixture.SetupProductDto();
            var command = new CreateProductCommand(productDto);

            _fixture.ProductsServiceClientMock_Setup_CreateProductAsync_Throw_Exception(expectedExceptionType, expectedErrorMessage);

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*{expectedErrorMessage}*");

            _fixture.ProductsServiceClientMock_Verify_CreateProductAsync(Times.Once());
        }
    }
}
