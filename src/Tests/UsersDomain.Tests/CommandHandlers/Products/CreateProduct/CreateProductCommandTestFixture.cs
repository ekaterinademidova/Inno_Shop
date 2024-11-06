using Moq;
using UsersApplication.Dtos;
using UsersApplication.Interfaces.HttpClientContracts;
using UsersApplication.Products.Commands.CreateProduct;

namespace UsersApplication.Tests.CommandHandlers.Products.CreateProduct
{
    public sealed class CreateProductCommandTestFixture : CommandHandlerFixtureBase
    {
        public CreateProductHandler CommandHandler { get; }
        public Mock<IProductsServiceClient> ProductsServiceClientMock { get; }

        public CreateProductCommandTestFixture()
        {
            ProductsServiceClientMock = new Mock<IProductsServiceClient>();
            CommandHandler = new CreateProductHandler(ProductsServiceClientMock.Object);
        }

        public CommandHandlerFixtureBase ProductsServiceClientMock_Setup_CreateProductAsync_Returns_ProductId(ProductDto productDto)
        {
            ProductsServiceClientMock
                .Setup(client => client.CreateProductAsync(productDto))
                .ReturnsAsync(Guid.NewGuid());
            return this;
        }

        public CommandHandlerFixtureBase ProductsServiceClientMock_Setup_CreateProductAsync_Throw_Exception(Type exceptionType, string errorMessage)
        {
            ProductsServiceClientMock
                .Setup(client => client.CreateProductAsync(It.IsAny<ProductDto>()))
                .ThrowsAsync((Exception)Activator.CreateInstance(exceptionType, errorMessage)!);

            return this;
        }

        public CommandHandlerFixtureBase ProductsServiceClientMock_Verify_CreateProductAsync(Times times)
        {
            ProductsServiceClientMock
                .Verify(client => client.CreateProductAsync(It.IsAny<ProductDto>()), times);

            return this;
        }

        public ProductDto SetupProductDto() =>
        new()
        {
            Name = "TestProductName",
            Description = "TestProductDescription",
            ImageFile = "TestProductImageFile",
            Price = 20.50M,
            Quantity = 10
        };
    }
}