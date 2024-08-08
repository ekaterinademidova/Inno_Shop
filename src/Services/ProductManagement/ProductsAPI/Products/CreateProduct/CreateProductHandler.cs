namespace ProductsAPI.Products.CreateProduct
{
    //public record CreateProductCommand(string Name, string Description, string ImageFile, decimal Price, int Quantity, Guid CreatedByUserId)
    //     : ICommand<CreateProductResult>;
    public record CreateProductCommand(ProductDto Product)
         : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(command => command.Product.Id).NotEmpty().WithMessage("Product Id is required");
            RuleFor(command => command.Product.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
            RuleFor(command => command.Product.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(command => command.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(command => command.Product.CreatedByUserId).NotEmpty().WithMessage("Creator ID is required");
        }
    }
    internal class CreateProductCommandHandler(IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // create Product entity from command object
            var product = new Product
            {
                Id = command.Product.Id,
                Name = command.Product.Name,
                Description = command.Product.Description,
                ImageFile = command.Product.ImageFile,
                Price = command.Product.Price,
                Quantity = command.Product.Quantity,
                CreatedByUserId = command.Product.CreatedByUserId,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            // save to database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            // return CreateProductResult result
            return new CreateProductResult(product.Id);
        }
    }
}
