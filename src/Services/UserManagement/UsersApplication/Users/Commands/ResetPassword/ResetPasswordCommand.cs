namespace UsersApplication.Users.Commands.ResetPassword
{
    public record ResetPasswordCommand(Guid Token, string NewPassword) : ICommand<ResetPasswordResult>;
    public record ResetPasswordResult(bool IsSuccess);
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(command => command.Token).NotNull().WithMessage("Token is required.");
            RuleFor(command => command.NewPassword)
                .NotEmpty().WithMessage("NewPassword is required")
                .Length(2, 100).WithMessage("NewPassword must be between 2 and 100 characters");
        }
    }
}
