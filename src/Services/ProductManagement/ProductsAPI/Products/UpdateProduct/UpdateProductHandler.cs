namespace ProductsAPI.Products.UpdateProduct
{
    //public record UpdateProductCommand(Guid Id, string Name, string Description, string ImageFile, decimal Price, int Quantity, Guid CreatedByUserId)
    //    : ICommand<UpdateProductResult>;
    public record UpdateProductCommand(ProductDto Product)
     : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Product.Id).NotEmpty().WithMessage("Product ID is required");
            RuleFor(command => command.Product.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
            RuleFor(x => x.Product.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(command => command.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
    internal class UpdateProductCommandHandler
        (IDocumentSession session)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Product.Id, cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException(command.Product.Id);
            }

            product.Name = command.Product.Name;
            product.Description = command.Product.Description;
            product.ImageFile = command.Product.ImageFile;
            product.Price = command.Product.Price;
            product.Quantity = command.Product.Quantity;
            product.LastModified = DateTime.UtcNow;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
