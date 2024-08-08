using BuildingBlocks.Messaging.Events;
using MassTransit;
using ProductsAPI.Products.UpdateProduct;

namespace ProductsAPI.EventHandlers.Integration
{
    public class UpdateProductEventHandler
        (ISender sender, ILogger<UpdateProductEventHandler> logger)
        : IConsumer<UpdateProductEvent>
    {
        public async Task Consume(ConsumeContext<UpdateProductEvent> context)
        {
            // ...
            logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

            var command = MapToUpdateProductCommand(context.Message);
            await sender.Send(command);
        }

        private UpdateProductCommand MapToUpdateProductCommand(UpdateProductEvent message)
        {
            // Update product with incoming event data
            var productDto = new ProductDto
            {        
                Id = message.ProductId,
                Name = message.Name,
                Description = message.Description,
                ImageFile = message.ImageFile,
                Price = message.Price,
                Quantity = message.Quantity,
                CreatedByUserId = message.CreatedByUserId
            };
            return new UpdateProductCommand(productDto);
        }
    }
}
