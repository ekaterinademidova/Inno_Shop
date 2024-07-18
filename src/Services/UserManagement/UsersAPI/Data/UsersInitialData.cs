using Marten.Schema;

namespace UsersAPI.Data
{
    public class UsersInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<User>().AnyAsync())
            {
                return;
            }

            // Marten UPSERT will cater for existing records
            session.Store<User>(GetPreconfiguredProducts());
            await session.SaveChangesAsync();
        }

        private static IEnumerable<User> GetPreconfiguredProducts() => new List<User>()
        {
            new User()
            {
                Id = new Guid("ac0e6691-8b4d-45a3-ae2b-582c88ada3bb"),
                Name = "SuperAdmin",
                Email = "inno.sa@gmail.com"
            }
        };
    }
}

