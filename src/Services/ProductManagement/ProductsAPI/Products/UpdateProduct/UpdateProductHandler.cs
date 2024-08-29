using Microsoft.AspNetCore.Http;
using ProductsAPI.Interfaces;

namespace ProductsAPI.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, string Description, string ImageFile, decimal Price, int Quantity)
        : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");
            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
            RuleFor(command => command.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(command => command.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
    internal class UpdateProductCommandHandler
        (IDocumentSession session, IHttpContextAccessor httpContextAccessor, IUsersServiceClient usersServiceClient)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken)
                ?? throw new ProductNotFoundException(command.Id);

            var userIdClaim = (httpContextAccessor.HttpContext?.User.FindFirst("userId"))
                ?? throw new UnauthorizedAccessException("User ID not found in token.");
            var userId = Guid.Parse(userIdClaim.Value);
            bool userExists = await usersServiceClient.GetUserByIdAsync(userId);

            if (!userExists || userId != product.CreatedByUserId)
            {
                throw new UnauthorizedAccessException("User is not authorized/allowed to update products.");
            }

            product.Name = command.Name;
            product.Description = command.Description;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;
            product.Quantity = command.Quantity;
            product.LastModified = DateTime.UtcNow;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
