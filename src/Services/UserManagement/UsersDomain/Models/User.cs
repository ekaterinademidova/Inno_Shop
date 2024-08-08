using System.ComponentModel;
using UsersDomain.Enums.Extensions;

namespace UsersDomain.Models
{
    public class User : Aggregate<UserId>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public UserRole Role { get; set; } = UserRole.User;
        public UserStatus Status { get; set; } = UserStatus.Inactive;

        public static User Create(UserId id, string firstName, string lastName, string email, string password, UserRole role)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            //ArgumentOutOfRangeException.ThrowIfGreaterThan((int)role, Enum.GetValues(typeof(UserRole)).Cast<int>().Max());
            //ArgumentOutOfRangeException.ThrowIfLessThan((int)role, EnumTraits<UserRole>.MinValue);
            //ArgumentOutOfRangeException.ThrowIfGreaterThan((int)role, EnumTraits<UserRole>.MaxValue);

            if (!EnumTraits<UserRole>.IsValid((role)))
                throw new InvalidEnumArgumentException("The role \"" + role.ToString() + "\" does not exist.");

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

        public void Update(string firstName, string lastName, string email, string password, UserRole role)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            if (!EnumTraits<UserRole>.IsValid((role)))
                throw new InvalidEnumArgumentException("The role \"" + role.ToString() + "\" does not exist.");

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = role;

            AddDomainEvent(new UserUpdatedEvent(this));
        }
    }
}
