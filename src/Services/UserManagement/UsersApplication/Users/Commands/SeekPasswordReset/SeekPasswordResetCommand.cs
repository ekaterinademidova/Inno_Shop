namespace UsersApplication.Users.Commands.SeekPasswordReset
{
    public record SeekPasswordResetCommand(string Email) : ICommand<SeekPasswordResetResult>;
    public record SeekPasswordResetResult(bool IsSuccess);
    public class SeekPasswordResetCommandValidator : AbstractValidator<SeekPasswordResetCommand>
    {
        public SeekPasswordResetCommandValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email address is invalid")
                .Length(2, 255).WithMessage("Email must be between 2 and 255 characters");                
        }
    }
}
