using Marten.Schema;

namespace ProductsAPI.Data
{
    public class ProductsInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync())
            {
                return;
            }

            // Marten UPSERT will cater for existing records
            session.Store<Product>(GetPreconfiguredProducts());
            await session.SaveChangesAsync();
        }

        private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>()
        {
            new Product()
            {
                Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                Name = "IPhone X",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-1.png",
                Price = 950.00M,
                Quantity = 20,
                CreatedByUserId = new Guid("ac0e6691-8b4d-45a3-ae2b-582c88ada3bb"),
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now
            },
            new Product()
            {
                Id = new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
                Name = "Samsung 10",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-2.png",
                Price = 840.00M,
                Quantity = 10,
                CreatedByUserId = new Guid("ac0e6691-8b4d-45a3-ae2b-582c88ada3bb"),
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now
            },
            new Product()
            {
                Id = new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"),
                Name = "Huawei Plus",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-3.png",
                Price = 650.00M,
                Quantity = 0,
                CreatedByUserId = new Guid("ac0e6691-8b4d-45a3-ae2b-582c88ada3bb"),
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now
            },
            new Product()
            {
                Id = new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"),
                Name = "Xiaomi Mi 9",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-4.png",
                Price = 470.00M,
                Quantity = 30,
                CreatedByUserId = new Guid("3e5af1a9-aa98-44e5-aadc-6b51dbdbc4c1"),
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now
            },
            new Product()
            {
                Id = new Guid("b786103d-c621-4f5a-b498-23452610f88c"),
                Name = "HTC U11+ Plus",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-5.png",
                Price = 380.00M,
                Quantity = 45,
                CreatedByUserId = new Guid("3e5af1a9-aa98-44e5-aadc-6b51dbdbc4c1"),
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now
            },
            new Product()
            {
                Id = new Guid("c4bbc4a2-4555-45d8-97cc-2a99b2167bff"),
                Name = "LG G7 ThinQ",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-6.png",
                Price = 240.00M,
                Quantity = 5,
                CreatedByUserId = new Guid("d9d62d60-2119-497b-aeef-56f3aa5ba45a"),
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now
            }
        };
    }
}
