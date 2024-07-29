namespace ProductsAPI.Products.GetProductsByCreator
{
    //public record GetProductByCreatorRequest();
    public record GetProductByCreatorResponse(IEnumerable<Product> Products);

    public class GetProductByCreatorEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/creator/{createdByUserId}", async (Guid createdByUserId, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCreatorQuery(createdByUserId));
                var response = result.Adapt<GetProductByCreatorResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProductByCreator")
            .Produces<GetProductByCreatorResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Product By Creator")
            .WithDescription("Get Product By Creator");
        }
    }
}
