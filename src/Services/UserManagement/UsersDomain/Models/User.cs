namespace UsersDomain.Models
{
    public class User : Aggregate<UserId>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public bool IsConfirmed { get; set; } = false;
        public UserRole Role { get; set; } = UserRole.User;

        private readonly List<OperationToken> _operationTokens = [];
        public IReadOnlyList<OperationToken> OperationTokens => _operationTokens.AsReadOnly();

        public string FullName => $"{FirstName} {LastName}";

        public static User Create(UserId id, string firstName, string lastName, string email, string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(id.Value.ToString());
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            //if (!EnumTraits<UserRole>.IsValid((role)))
            //    throw new InvalidEnumArgumentException("The role \"" + role.ToString() + "\" does not exist.");

            var user = new User
            {
                Id = id //UserId.Of(Guid.NewGuid())
            };

            user.SetFirstName(firstName);
            user.SetLastName(lastName);
            user.SetEmail(email);
            user.SetPassword(password);
            //user.SetRole(role);

            user.AddDomainEvent(new UserCreatedEvent(user));

            return user;
        }

        public void Update(string firstName, string lastName, string email, string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            //if (!EnumTraits<UserRole>.IsValid((role)))
            //    throw new InvalidEnumArgumentException("The role \"" + role.ToString() + "\" does not exist.");

            SetFirstName(firstName);
            SetLastName(lastName);
            SetEmail(email);
            SetPassword(password);
            //SetRole(role);

            AddDomainEvent(new UserUpdatedEvent(this));
        }

        public void SetFirstName(string firstName)
        {
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            LastName = lastName;
        }
        public void SetEmail(string email)
        {
            Email = email;
        }

        public void SetPassword(string password)
        {
            Password = password;
        }

        public void SetConfirmation(bool isConfirmed)
        {
            IsConfirmed = isConfirmed;
        }

        public void SetRole(UserRole role)
        {
            Role = role;
        }
    }
}
