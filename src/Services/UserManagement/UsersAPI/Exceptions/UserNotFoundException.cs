namespace UsersAPI.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(Guid Id) : base("User not found!") { }
    }
}
