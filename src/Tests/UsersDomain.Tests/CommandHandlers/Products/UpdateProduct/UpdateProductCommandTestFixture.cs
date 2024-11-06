using Moq;
using UsersApplication.Dtos;
using UsersApplication.Interfaces.HttpClientContracts;
using UsersApplication.Products.Commands.UpdateProduct;

namespace UsersApplication.Tests.CommandHandlers.Products.UpdateProduct
{
    public sealed class UpdateProductCommandTestFixture : CommandHandlerFixtureBase
    {
        public UpdateProductHandler CommandHandler { get; }
        public Mock<IProductsServiceClient> ProductsServiceClientMock { get; }

        public UpdateProductCommandTestFixture()
        {
            ProductsServiceClientMock = new Mock<IProductsServiceClient>();
            CommandHandler = new UpdateProductHandler(ProductsServiceClientMock.Object);
        }

        public CommandHandlerFixtureBase ProductsServiceClientMock_Setup_UpdateProductAsync_Returns_True(ProductDto productDto)
        {
            ProductsServiceClientMock
                .Setup(client => client.UpdateProductAsync(productDto))
                .ReturnsAsync(true);
            return this;
        }

        public CommandHandlerFixtureBase ProductsServiceClientMock_Setup_UpdateProductAsync_Throw_Exception(Type exceptionType, string errorMessage)
        {
            ProductsServiceClientMock
                .Setup(client => client.UpdateProductAsync(It.IsAny<ProductDto>()))
                .ThrowsAsync((Exception)Activator.CreateInstance(exceptionType, errorMessage)!);

            return this;
        }

        public CommandHandlerFixtureBase ProductsServiceClientMock_Verify_UpdateProductAsync(Times times)
        {
            ProductsServiceClientMock
                .Verify(client => client.UpdateProductAsync(It.IsAny<ProductDto>()), times);

            return this;
        }

        public ProductDto SetupProductDto() =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = "TestProductName",
            Description = "TestProductDescription",
            ImageFile = "TestProductImageFile",
            Price = 20.50M,
            Quantity = 10
        };
    }
}