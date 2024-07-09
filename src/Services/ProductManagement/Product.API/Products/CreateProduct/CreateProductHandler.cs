using MediatR;

namespace Product.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, string ImageFile, decimal Price, int Quantity, Guid CreatedByUserId, DateTime CreatedDate, DateTime LastModified)
         :IRequest<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        public Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
