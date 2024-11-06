using ProductsAPI.HttpClients;
using ProductsAPI.Interfaces.HttpClientContracts;
using ProductsAPI.Interfaces.ServiceContracts;
using System.Security.Claims;

namespace ProductsAPI.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");
        }
    }
    internal class DeleteProductCommandHandler
        (IDocumentSession session, IApiUserService apiUserService, IUsersServiceClient usersServiceClient)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken)
                ?? throw new ProductNotFoundException(command.Id);

            //var userIdClaim = (httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier))
            //    ?? throw new UnauthorizedAccessException("User ID not found in token.");
            //var userId = Guid.Parse(userIdClaim.Value);
            var userId = apiUserService.GetUserId();
            bool userExists = await usersServiceClient.GetUserByIdAsync(userId);

            if (!userExists || userId != product.CreatedByUserId)
            {
                throw new UnauthorizedAccessException("User is not authorized/allowed to update products.");
            }

            session.Delete<Product>(command.Id);
            await session.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
