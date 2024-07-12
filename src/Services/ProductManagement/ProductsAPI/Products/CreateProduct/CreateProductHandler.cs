namespace ProductsAPI.Products.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, string ImageFile, decimal Price, int Quantity, Guid CreatedByUserId/*, DateTime CreatedDate, DateTime LastModified*/)
         : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    internal class CreateProductCommandHandler
        (IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // create Product entity from command object
            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
                Quantity = command.Quantity,
                CreatedByUserId = command.CreatedByUserId,
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now
            };

            // save to database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            // return CreateProductResult result
            return new CreateProductResult(product.Id);
        }
    }
}
