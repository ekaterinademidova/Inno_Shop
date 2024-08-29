namespace ProductsAPI.Products.SearchProducts
{
    //public record  SearchProductsByNameRequest();
    public record SearchProductsResponse(IEnumerable<Product> Products);

    public class SearchProductsBEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/search/{term}", async (string term, ISender sender) =>
            {
                var result = await sender.Send(new SearchProductsQuery(term));
                var response = result.Adapt<SearchProductsResponse>();
                return Results.Ok(response);
            })
            .WithName("SearchProducts")
            .Produces<SearchProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Search Products By Name")
            .WithDescription("Search Products By Name");
        }
    }
}
