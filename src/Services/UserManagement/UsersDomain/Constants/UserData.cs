namespace UsersDomain.Constants
{
    public static class UserData
    {
        public static class Ids
        {
            public static readonly UserId AdminId = UserId.Of(new Guid("ac0e6691-8b4d-45a3-ae2b-582c88ada3bb"));
            public static readonly UserId User1Id = UserId.Of(new Guid("3e5af1a9-aa98-44e5-aadc-6b51dbdbc4c1"));
            public static readonly UserId User2Id = UserId.Of(new Guid("d9d62d60-2119-497b-aeef-56f3aa5ba45a"));
            public static readonly UserId User3Id = UserId.Of(new Guid("096254eb-8e97-4dd1-bbef-43ed7a65ebcc"));

        }
        public static class Emails
        {

            public static readonly string AdminEmail = "admin@gmail.com";
            public static readonly string User1Email = "user1@gmail.com";
            public static readonly string User2Email = "user2@gmail.com";
            public static readonly string User3Email = "user3@gmail.com";

        }
        public static class Passwords
        {
            public static readonly string AdminPassword = "user0Password";
            public static readonly string User1Password = "user1Password";
            public static readonly string User2Password = "user2Password";
            public static readonly string User3Password = "user3Password";

        }
    }
}