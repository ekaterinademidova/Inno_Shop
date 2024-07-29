namespace UsersDomain.Models
{
    public class Product : Entity<ProductId>
    {
        internal Product(string name, string description, decimal price, int quantity, UserId createdByUserId)
        {
            Id = ProductId.Of(Guid.NewGuid());
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
            CreatedByUserId = createdByUserId;
        }

        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public int Quantity { get; set; } = default!;
        public UserId CreatedByUserId { get; set; } = default!;
        public DateTime CreatedDate { get; set; } = default!;
        public DateTime LastModified { get; set; } = default!;

        //public static Product Create(ProductId id, string name, string description, decimal price, int quantity, UserId createdByUserId)
        //{
        //    ArgumentException.ThrowIfNullOrWhiteSpace(name);
        //    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
        //    ArgumentNullException.ThrowIfNull(createdByUserId);

        //    var product = new Product
        //    {
        //        Id = id,
        //        Name = name,
        //        Description = description,
        //        Price = price,
        //        Quantity = quantity,
        //        CreatedByUserId = createdByUserId
        //    };

        //    return product;
        //}
    }
}
