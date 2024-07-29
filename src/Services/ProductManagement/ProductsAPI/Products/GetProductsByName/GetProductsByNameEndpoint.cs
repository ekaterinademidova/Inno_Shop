namespace ProductsAPI.Products.GetProductsByName
{
    //public record GetProductByNameRequest();
    public record GetProductByNameResponse(IEnumerable<Product> Products);

    public class GetProductByNameEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/name/{name}", async (string name, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByNameQuery(name));
                var response = result.Adapt<GetProductByNameResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProductByName")
            .Produces<GetProductByNameResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Product By Name")
            .WithDescription("Get Product By Name");
        }
    }
}
