using Moq;
using UsersApplication.Interfaces.HttpClientContracts;
using UsersApplication.Products.Commands.DeleteProduct;

namespace UsersApplication.Tests.CommandHandlers.Products.DeleteProduct
{
    public sealed class DeleteProductCommandTestFixture : CommandHandlerFixtureBase
    {
        public DeleteProductHandler CommandHandler { get; }
        public Mock<IProductsServiceClient> ProductsServiceClientMock { get; }

        public DeleteProductCommandTestFixture()
        {
            ProductsServiceClientMock = new Mock<IProductsServiceClient>();
            CommandHandler = new DeleteProductHandler(ProductsServiceClientMock.Object);
        }

        public CommandHandlerFixtureBase ProductsServiceClientMock_Setup_DeleteProductAsync_Returns_True(Guid id)
        {
            ProductsServiceClientMock
                .Setup(client => client.DeleteProductAsync(id))
                .ReturnsAsync(true);

            return this;
        }

        public CommandHandlerFixtureBase ProductsServiceClientMock_Setup_DeleteProductAsync_Throw_Exception(Type exceptionType, string errorMessage)
        {
            ProductsServiceClientMock
                .Setup(client => client.DeleteProductAsync(It.IsAny<Guid>()))
                .ThrowsAsync((Exception)Activator.CreateInstance(exceptionType, errorMessage)!);

            return this;
        }

        public CommandHandlerFixtureBase ProductsServiceClientMock_Verify_DeleteProductAsync(Times times)
        {
            ProductsServiceClientMock
                .Verify(client => client.DeleteProductAsync(It.IsAny<Guid>()), times);

            return this;
        }
    }
}