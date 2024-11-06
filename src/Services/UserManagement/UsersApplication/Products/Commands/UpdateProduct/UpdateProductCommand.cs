namespace UsersApplication.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(ProductDto Product) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            AddRuleForId();
            AddRuleForName();
            AddRuleForImageFile();
            AddRuleForPrice();
        }

        private void AddRuleForId()
        {
            RuleFor(cmd => cmd.Product.Id)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.Product.EmptyId)
                .WithMessage("Product id may not be empty");
        }

        private void AddRuleForName()
        {
            RuleFor(cmd => cmd.Product.Name)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.Product.EmptyName)
                .WithMessage("Name may not be empty")
                .MinimumLength(4)
                .WithErrorCode(DomainErrorCodes.Product.ShortName)
                .WithMessage($"Name may not be shorter than {4} characters")
                .MaximumLength(MaxLengths.Product.Name)
                .WithErrorCode(DomainErrorCodes.Product.LongName)
                .WithMessage($"Name may not be longer than {MaxLengths.Product.Name} characters");
        }

        private void AddRuleForImageFile()
        {
            RuleFor(cmd => cmd.Product.ImageFile)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.Product.EmptyImageFile)
                .WithMessage("ImageFile may not be empty");
        }

        private void AddRuleForPrice()
        {
            RuleFor(cmd => cmd.Product.Price)
                .GreaterThan(0)
                .WithErrorCode(DomainErrorCodes.Product.PriceLessThanZero)
                .WithMessage("Price must be greater than 0");
        }
    }
}
