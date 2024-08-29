using UsersApplication.Models;

namespace UsersApplication.Users.Commands.LoginUser
{
    public record LoginUserCommand(string Email, string Password) : ICommand<LoginUserResult>;
    public record LoginUserResult(Token Token);
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("Email is required")
                .Length(2, 255).WithMessage("Email must be between 2 and 255 characters");
            RuleFor(command => command.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(2, 100).WithMessage("Password must be between 2 and 100 characters");
        }
    }


    //public LoginUserCommandValidation()
    //{
    //    AddRuleForEmail();
    //    AddRuleForPassword();
    //}

    //private void AddRuleForEmail()
    //{
    //    RuleFor(cmd => cmd.Email)
    //        .EmailAddress()
    //        .WithErrorCode(DomainErrorCodes.User.InvalidEmail)
    //        .WithMessage("Email is not a valid email address")
    //        .MaximumLength(MaxLengths.User.Email)
    //        .WithErrorCode(DomainErrorCodes.User.EmailExceedsMaxLength)
    //        .WithMessage($"Email may not be longer than {MaxLengths.User.Email} characters");
    //}

    //private void AddRuleForPassword()
    //{
    //    RuleFor(cmd => cmd.Password)
    //        .Password();
    //}
}
