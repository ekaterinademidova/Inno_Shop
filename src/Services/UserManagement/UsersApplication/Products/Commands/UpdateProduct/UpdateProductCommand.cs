namespace UsersApplication.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(ProductDto Product) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Product.Id).NotEmpty().WithMessage("ProductId is required");
            RuleFor(command => command.Product.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
            RuleFor(command => command.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(command => command.Product.CreatedByUserId).NotEmpty().WithMessage("Creator ID is required");
        }
    }
}
