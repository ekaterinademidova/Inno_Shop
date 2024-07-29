namespace UsersDomain.Models
{
    public class User : Aggregate<UserId>
    {
        private /*readonly*/ List<Product> _createdProducts = new();
        public IReadOnlyList<Product> CreatedProducts => _createdProducts.AsReadOnly();
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public UserRole Role { get; set; } = UserRole.User;
        public UserStatus Status { get; set; } = UserStatus.Inactive;

        public static User Create(UserId id, string firstName, string lastName, string email, string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            var user = new User
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };

            user.AddDomainEvent(new UserCreatedEvent(user));

            return user;
        }

        public void Update(string firstName, string lastName, string email, string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);


            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;

            AddDomainEvent(new UserUpdatedEvent(this));
        }

        public void CreateProduct(string name, string description, decimal price, int quantity)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            //var productId = ProductId.Of(Guid.NewGuid());
            var createdByUserId = Id;
            //var product = Product.Create(productId, name, description, price, quantity, createdByUserId);
            var product = new Product(name, description, price, quantity, createdByUserId);

            _createdProducts.Add(product);
            AddDomainEvent(new CreateProductEvent(this, product));
        }

        public void UpdateProduct(ProductId productId, string name, string description, decimal price, int quantity)
        {
            var product = _createdProducts.FirstOrDefault(x => x.Id == productId);

            if (product is null)
            {
                ArgumentNullException.ThrowIfNull(product, "Product is not found.");
            }

            product.Name = name;
            product.Description = description;
            product.Price = price;
            product.Quantity = quantity;
            AddDomainEvent(new UpdateProductEvent(this, product));
        }

        public void DeleteProduct(ProductId productId)
        {
            var product = _createdProducts.FirstOrDefault(x => x.Id == productId);
            if (product is not null)
            {
                _createdProducts.Remove(product);
                AddDomainEvent(new DeleteProductEvent(this, productId));
            }                       
        }
    }
}
