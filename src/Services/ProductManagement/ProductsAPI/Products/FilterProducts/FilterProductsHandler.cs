namespace ProductsAPI.Products.FilterProducts
{
    public record FilterProductsQuery(decimal? MinPrice = null, decimal? MaxPrice = null, bool InStock = false, Guid? CreatedByUserId = null)
        : IQuery<FilterProductsResult>;
    public record FilterProductsResult(IEnumerable<Product> Products);

    internal class FilterProductsQueryHandler(IDocumentSession session)
        : IQueryHandler<FilterProductsQuery, FilterProductsResult>
    {
        public async Task<FilterProductsResult> Handle(FilterProductsQuery query, CancellationToken cancellationToken)
        {
            if (query.MinPrice.HasValue && query.MaxPrice.HasValue && query.MinPrice.Value > query.MaxPrice.Value)
            {
                throw new ArgumentException("Минимальная цена не может быть больше максимальной цены.");
            }

            var products = session.Query<Product>().AsQueryable();

            if (query.MinPrice.HasValue)
                products = products.Where(p => p.Price >= query.MinPrice.Value);

            if (query.MaxPrice.HasValue)
                products = products.Where(p => p.Price <= query.MaxPrice.Value);

            if (query.InStock)
                products = products.Where(p => p.Quantity > 0);

            if (query.CreatedByUserId.HasValue)
                    products = products.Where(p => p.CreatedByUserId == query.CreatedByUserId);

            return new FilterProductsResult(await products.ToListAsync(cancellationToken));
        }
    }
}
