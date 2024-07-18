namespace UsersAPI.Users.UpdateUser
{
    public record UpdateUserCommand(Guid Id, string Name, string Email)
        : ICommand<UpdateUserResult>;
    public record UpdateUserResult(bool IsSuccess);
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("User ID is required");
            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
            RuleFor(command => command.Email).NotEmpty().WithMessage("Email is required");
        }
    }
    internal class UpdateUserCommandHandler
        (IDocumentSession session)
        : ICommandHandler<UpdateUserCommand, UpdateUserResult>
    {
        public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await session.LoadAsync<User>(command.Id, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(command.Id);
            }

            user.Name = command.Name;
            user.Email = command.Email;

            session.Update(user);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateUserResult(true);
        }
    }
}
