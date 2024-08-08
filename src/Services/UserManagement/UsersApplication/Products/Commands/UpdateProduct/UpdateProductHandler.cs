using BuildingBlocks.Messaging.Events;
using Mapster;
using MassTransit;

namespace UsersApplication.Products.Commands.UpdateProduct
{
    public class UpdateProductHandler
        (IApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(command.Product.CreatedByUserId);
            var user = await dbContext.Users
                .FindAsync([userId], cancellationToken: cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(command.Product.CreatedByUserId);
            }

            // set event message
            var productId = command.Product.Id;
            var eventMessage = command.Product.Adapt<UpdateProductEvent>();            
            eventMessage.ProductId = productId;

            // send update-product event to RabbitMQ using MassTransit
            await publishEndpoint.Publish(eventMessage, cancellationToken);

            // return UpdateProductResult result
            return new UpdateProductResult(true);
        }
    }
}
