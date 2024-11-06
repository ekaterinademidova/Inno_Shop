using FluentAssertions;
using Moq;
using UsersApplication.Products.Commands.UpdateProduct;

namespace UsersApplication.Tests.CommandHandlers.Products.UpdateProduct
{
    public sealed class UpdateProductCommandHandlerTests
    {
        private readonly UpdateProductCommandTestFixture _fixture = new();

        [Fact]
        public async Task Should_Update_Product()
        {
            // Arrange
            var productDto = _fixture.SetupProductDto();
            var command = new UpdateProductCommand(productDto);

            _fixture.ProductsServiceClientMock_Setup_UpdateProductAsync_Returns_True(productDto);

            // Act
            var result = await _fixture.CommandHandler.Handle(command, default);

            // Assert
            _fixture.ProductsServiceClientMock_Verify_UpdateProductAsync(Times.Once());

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException), "Service error.")]
        public async Task Should_Throw_Exception_For_Service_Error(Type expectedExceptionType, string expectedErrorMessage)
        {
            // Arrange
            var productDto = _fixture.SetupProductDto();
            var command = new UpdateProductCommand(productDto);

            _fixture.ProductsServiceClientMock_Setup_UpdateProductAsync_Throw_Exception(expectedExceptionType, expectedErrorMessage);

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*{expectedErrorMessage}*");

            _fixture.ProductsServiceClientMock_Verify_UpdateProductAsync(Times.Once());
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException), "Network error.")]
        public async Task Should_Throw_Exception_For_Network_Error(Type expectedExceptionType, string expectedErrorMessage)
        {
            // Arrange
            var productDto = _fixture.SetupProductDto();
            var command = new UpdateProductCommand(productDto);

            _fixture.ProductsServiceClientMock_Setup_UpdateProductAsync_Throw_Exception(expectedExceptionType, expectedErrorMessage);

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*{expectedErrorMessage}*");

            _fixture.ProductsServiceClientMock_Verify_UpdateProductAsync(Times.Once());
        }

        [Theory]
        [InlineData(typeof(TimeoutException), "The request timed out.")]
        public async Task Should_Throw_Exception_For_Timeout_Error(Type expectedExceptionType, string expectedErrorMessage)
        {
            // Arrange
            var productDto = _fixture.SetupProductDto();
            var command = new UpdateProductCommand(productDto);

            _fixture.ProductsServiceClientMock_Setup_UpdateProductAsync_Throw_Exception(expectedExceptionType, expectedErrorMessage);

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<TimeoutException>()
                .WithMessage($"*{expectedErrorMessage}*");

            _fixture.ProductsServiceClientMock_Verify_UpdateProductAsync(Times.Once());
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException), "Failed to deserialize the response or the response is null.")]
        public async Task Should_Throw_Exception_For_Json_Error(Type expectedExceptionType, string expectedErrorMessage)
        {
            // Arrange
            var productDto = _fixture.SetupProductDto();
            var command = new UpdateProductCommand(productDto);

            _fixture.ProductsServiceClientMock_Setup_UpdateProductAsync_Throw_Exception(expectedExceptionType, expectedErrorMessage);

            // Act
            Func<Task> action = () => _fixture.CommandHandler.Handle(command, default);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*{expectedErrorMessage}*");

            _fixture.ProductsServiceClientMock_Verify_UpdateProductAsync(Times.Once());
        }
    }
}
