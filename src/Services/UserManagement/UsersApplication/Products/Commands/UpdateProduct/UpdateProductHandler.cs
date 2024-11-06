using UsersApplication.Interfaces.HttpClientContracts;

namespace UsersApplication.Products.Commands.UpdateProduct
{
    public class UpdateProductHandler(IProductsServiceClient productsServiceClient)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            bool isSuccess = await productsServiceClient.UpdateProductAsync(command.Product);

            return new UpdateProductResult(isSuccess);
        }
    }
}
