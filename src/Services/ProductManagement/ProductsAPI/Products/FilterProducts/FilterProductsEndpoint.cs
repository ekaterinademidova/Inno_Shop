namespace ProductsAPI.Products.FilterProducts
{
    public record FilterProductsRequest(decimal? MinPrice = null, decimal? MaxPrice = null, bool InStock = false, Guid? CreatedByUserId = null);
    public record FilterProductsResponse(IEnumerable<Product> Products);

    public class FilterProductsBEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/filter", async ([AsParameters] FilterProductsRequest request, ISender sender) =>
            {
                var query = request.Adapt<FilterProductsQuery>();
                var result = await sender.Send(query);
                var response = result.Adapt<FilterProductsResponse>();

                return Results.Ok(response);
            })
            .WithName("FilterProducts")
            .Produces<FilterProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Filter Products By Name")
            .WithDescription("Filter Products By Name");
        }
    }
}
