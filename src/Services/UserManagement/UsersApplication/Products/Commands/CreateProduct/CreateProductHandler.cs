using BuildingBlocks.Messaging.Events;
using Mapster;
using MassTransit;

namespace UsersApplication.Products.Commands.CreateProduct
{
    public class CreateProductHandler
        (IApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(command.Product.CreatedByUserId);
            var user = await dbContext.Users
                .FindAsync([userId], cancellationToken: cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(command.Product.CreatedByUserId);
            }

            // set event message
            var productId = Guid.NewGuid();
            var eventMessage = command.Product.Adapt<CreateProductEvent>();
            eventMessage.ProductId = productId;

            // send create-product event to RabbitMQ using MassTransit
            await publishEndpoint.Publish(eventMessage, cancellationToken);

            // return CreateProductResult result
            return new CreateProductResult(productId);
        }
    }
}
