namespace UsersApplication.Products.Commands.CreateProduct
{
    public record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid ProductId);
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(command => command.Product.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
            RuleFor(command => command.Product.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(command => command.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
}
