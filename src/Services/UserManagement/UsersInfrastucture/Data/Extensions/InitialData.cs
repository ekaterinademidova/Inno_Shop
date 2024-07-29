namespace UsersInfrastucture.Data.Extensions
{
    internal class InitialData
    {
        public static IEnumerable<User> UsersWithProducts
        {
            get
            {
                var user1 = User.Create(
                                UserId.Of(Guid.NewGuid()),
                                "UserFirstName 1",
                                "UserLastName 1",
                                "user1email@gmail.com",
                                "userPassword1");

                user1.CreateProduct("ProductName 1", "ProductDescription 1", 200.00M, 10);
                user1.CreateProduct("ProductName 2", "ProductDescription 2", 150.00M, 30);

                var user2 = User.Create(
                                UserId.Of(Guid.NewGuid()),
                                "UserFirstName 2",
                                "UserLastName 2",
                                "user2email@gmail.com",
                                "userPassword2");

                user2.CreateProduct("ProductName 3", "ProductDescription 3", 550.00M, 5);
                user2.CreateProduct("ProductName 4", "ProductDescription 4", 50.00M, 20);

                var user3 = User.Create(
                                UserId.Of(Guid.NewGuid()),
                                "UserFirstName 3",
                                "UserLastName 3",
                                "user3email@gmail.com",
                                "userPassword3");

                return new List<User> { user1, user2, user3 };
            }
        }
    }
}
