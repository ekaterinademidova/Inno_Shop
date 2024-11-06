using UsersApplication.ValueObjects;

namespace UsersApplication.Users.Commands.LoginUser
{
    public record LoginUserCommand(string Email, string Password) : ICommand<LoginUserResult>;
    public record LoginUserResult(JwtToken Token);
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            AddRuleForEmail();
            AddRuleForPassword();
        }

        private void AddRuleForEmail()
        {
            RuleFor(cmd => cmd.Email)
                .EmailAddress()
                .WithErrorCode(DomainErrorCodes.User.InvalidEmail)
                .WithMessage("Email is not a valid email address")
                .MaximumLength(MaxLengths.User.Email)
                .WithErrorCode(DomainErrorCodes.User.EmailExceedsMaxLength)
                .WithMessage($"Email may not be longer than {MaxLengths.User.Email} characters");
        }

        private void AddRuleForPassword()
        {
            RuleFor(cmd => cmd.Password)
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
