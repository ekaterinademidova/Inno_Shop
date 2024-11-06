namespace UsersApplication.Extensions
{
    public static class UserExtensions
    {
        public static IEnumerable<UserDto> ToUserDtoList(this IEnumerable<User> users)
        {
            return users.Select(user => new UserDto {
                Id = user.Id.Value,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                //Role = user.Role
            });
        }

        public static UserDto ToUserDto(this User user)
        {
            return DtoFromUser(user);
        }

        private static UserDto DtoFromUser(User user)
        {
            return new UserDto {
                Id = user.Id.Value,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                //Role = user.Role
            };
        }
    }
}
