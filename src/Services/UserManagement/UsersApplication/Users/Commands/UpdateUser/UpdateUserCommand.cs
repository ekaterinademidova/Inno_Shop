namespace UsersApplication.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(UserDto User)
        : ICommand<UpdateUserResult>;
    public record UpdateUserResult(bool IsSuccess);
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            AddRuleForId();
            AddRuleForFirstName();
            AddRuleForLastName();
            AddRuleForEmail();
            AddRuleForPassword();
        }

        private void AddRuleForId()
        {
            RuleFor(cmd => cmd.User.Id)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.User.EmptyId)
                .WithMessage("User id may not be empty");
        }

        private void AddRuleForFirstName()
        {
            RuleFor(cmd => cmd.User.FirstName)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.User.EmptyFirstName)
                .WithMessage("FirstName may not be empty")
                .MaximumLength(MaxLengths.User.FirstName)
                .WithErrorCode(DomainErrorCodes.User.FirstNameExceedsMaxLength)
                .WithMessage($"FirstName may not be longer than {MaxLengths.User.FirstName} characters");
        }

        private void AddRuleForLastName()
        {
            RuleFor(cmd => cmd.User.LastName)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.User.EmptyLastName)
                .WithMessage("LastName may not be empty")
                .MaximumLength(MaxLengths.User.LastName)
                .WithErrorCode(DomainErrorCodes.User.LastNameExceedsMaxLength)
                .WithMessage($"LastName may not be longer than {MaxLengths.User.LastName} characters");
        }

        private void AddRuleForEmail()
        {
            RuleFor(cmd => cmd.User.Email)
                .EmailAddress()
                .WithErrorCode(DomainErrorCodes.User.InvalidEmail)
                .WithMessage("Email is not a valid email address")
                .MaximumLength(MaxLengths.User.Email)
                .WithErrorCode(DomainErrorCodes.User.EmailExceedsMaxLength)
                .WithMessage($"Email may not be longer than {MaxLengths.User.Email} characters");
        }

        private void AddRuleForPassword()
        {
            RuleFor(cmd => cmd.User.Password)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.User.EmptyPassword)
                .WithMessage("Password may not be empty")
                .MinimumLength(8)
                .WithErrorCode(DomainErrorCodes.User.ShortPassword)
                .WithMessage($"Password may not be shorter than {8} characters")
                .MaximumLength(MaxLengths.User.Password)
                .WithErrorCode(DomainErrorCodes.User.LongPassword)
                .WithMessage($"Password may not be longer than {MaxLengths.User.Password} characters");
        }
    }
}
