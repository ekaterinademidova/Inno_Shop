using ProductsAPI.Interfaces.HttpClientContracts;
using ProductsAPI.Interfaces.ServiceContracts;

namespace ProductsAPI.Products.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, string ImageFile, decimal Price, int Quantity)
         : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
            RuleFor(command => command.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(command => command.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
    internal class CreateProductCommandHandler
        (IDocumentSession session, IApiUserService apiUserService, IUsersServiceClient usersServiceClient)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var userId = apiUserService.GetUserId();

            bool userExists = await usersServiceClient.GetUserByIdAsync(userId);
            if (!userExists)
            {
                throw new UnauthorizedAccessException("User is not authorized to create products.");
            }
                        
            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
                Quantity = command.Quantity,
                CreatedByUserId = userId,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }
    }
}
