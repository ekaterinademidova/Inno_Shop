//using BuildingBlocks.Messaging.Events;
//using MassTransit;
//using ProductsAPI.Products.CreateProduct;

//namespace ProductsAPI.EventHandlers.Integration
//{
//    public class CreateProductEventHandler
//        (ISender sender, ILogger<CreateProductEventHandler> logger)
//        : IConsumer<CreateProductEvent>
//    {
//        public async Task Consume(ConsumeContext<CreateProductEvent> context)
//        {
//            // ...
//            logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

//            var command = MapToCreateProductCommand(context.Message);
//            await sender.Send(command);
//        }

//        private CreateProductCommand MapToCreateProductCommand(CreateProductEvent message)
//        {
//            // Create product with incoming event data
//            var productDto = new ProductDto
//            {        
//                Id = message.ProductId,
//                Name = message.Name,
//                Description = message.Description,
//                ImageFile = message.ImageFile,
//                Price = message.Price,
//                Quantity = message.Quantity,
//                CreatedByUserId = message.CreatedByUserId
//            };
//            return new CreateProductCommand(productDto);
//            //return new CreateProductCommand(message.Name, message.Description, message.ImageFile, message.Price, message.Quantity, message.CreatedByUserId);
//        }
//    }
//}
