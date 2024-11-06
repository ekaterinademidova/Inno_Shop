using UsersApplication.Interfaces.HttpClientContracts;

namespace UsersApplication.Products.Commands.DeleteProduct
{
    public class DeleteProductHandler(IProductsServiceClient productsServiceClient)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            bool isSuccess = await productsServiceClient.DeleteProductAsync(command.Id);

            return new DeleteProductResult(isSuccess);
        }
    }
}
