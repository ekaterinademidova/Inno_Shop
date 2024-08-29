namespace ProductsAPI.Products.SearchProducts
{
    public record SearchProductsQuery(string Term) : IQuery<SearchProductsResult>;
    public record SearchProductsResult(IEnumerable<Product> Products);

    internal class SearchProductsQueryHandler
        (IDocumentSession session)
        : IQueryHandler<SearchProductsQuery, SearchProductsResult>
    {
        public async Task<SearchProductsResult> Handle(SearchProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>()
                .Where(p => p.Name.Contains(query.Term, StringComparison.OrdinalIgnoreCase) || 
                            p.Description.Contains(query.Term, StringComparison.OrdinalIgnoreCase))
                .ToListAsync(cancellationToken);

            return new SearchProductsResult(products);
        }
    }
}
