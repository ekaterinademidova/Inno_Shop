using UsersApplication.Interfaces.HttpClients;

namespace UsersApplication.Products.Commands.DeleteProduct
{
    public class DeleteProductHandler(IProductsServiceClient productsServiceClient)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            bool isSuccess = await productsServiceClient.DeleteProductAsync(command.Id);

            // return DeleteProductResult result
            return new DeleteProductResult(isSuccess);
        }
    }
}
