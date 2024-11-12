namespace UsersApplication.Dtos
{
    public class UserDto
    { 
        public Guid Id { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public bool IsConfirmed { get; set; } = false;
        //public UserRole Role { get; set; } = UserRole.User;
    }
}
