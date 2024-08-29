using UsersApplication.Interfaces.HttpClients;

namespace UsersApplication.Products.Commands.CreateProduct
{
    public class CreateProductHandler(IProductsServiceClient productsServiceClient)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            Guid productId = await productsServiceClient.CreateProductAsync(command.Product);

            // return CreateProductResult result
            return new CreateProductResult(productId);
        }
    }
}
