namespace UsersInfrastructure.Data.Extensions
{
    public class InitialData
    {
        public static IEnumerable<User> Users
        {
            get
            {
                var user0 = User.Create(
                                UserId.Of(new Guid("ac0e6691-8b4d-45a3-ae2b-582c88ada3bb")),
                                "UserFirstName 0",
                                "UserLastName 0",
                                "user0email@gmail.com",
                                "user0Password");//,
                                                 //UserRole.Admin);
                user0.SetRole(UserRole.Admin);

                var user1 = User.Create(
                                UserId.Of(new Guid("3e5af1a9-aa98-44e5-aadc-6b51dbdbc4c1")),
                                "UserFirstName 1",
                                "UserLastName 1",
                                "user1email@gmail.com",
                                "user1Password");//,
                                                 //UserRole.User);

                var user2 = User.Create(
                                UserId.Of(new Guid("d9d62d60-2119-497b-aeef-56f3aa5ba45a")),
                                "UserFirstName 2",
                                "UserLastName 2",
                                "user2email@gmail.com",
                                "user2Password");//,
                                                 //UserRole.User);

                var user3 = User.Create(
                                UserId.Of(new Guid("096254eb-8e97-4dd1-bbef-43ed7a65ebcc")),
                                "UserFirstName 3",
                                "UserLastName 3",
                                "user3email@gmail.com",
                                "user3Password");//,
                                                 //UserRole.User);

                return [user0, user1, user2, user3];
            }
        }
    }
}
