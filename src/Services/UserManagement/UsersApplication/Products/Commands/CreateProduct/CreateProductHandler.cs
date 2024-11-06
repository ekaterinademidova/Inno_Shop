using UsersApplication.Interfaces.HttpClientContracts;

namespace UsersApplication.Products.Commands.CreateProduct
{
    public class CreateProductHandler(IProductsServiceClient productsServiceClient)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var productId = await productsServiceClient.CreateProductAsync(command.Product);

            return new CreateProductResult(productId);
        }
    }
}
