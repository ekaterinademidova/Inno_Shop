namespace UsersApplication.Users.Commands.ConfirmUserEmail
{
    public class ConfirmUserEmailHandler(IUnitOfWork unitOfWork)
        : ICommandHandler<ConfirmUserEmailCommand, ConfirmUserEmailResult>
    {
        public async Task<ConfirmUserEmailResult> Handle(ConfirmUserEmailCommand command, CancellationToken cancellationToken)
        {
            var operationType = OperationType.EmailConfirmation;

            var token = await unitOfWork.OperationToken
                .GetAsync(filter: t => t.Code == command.Token && t.OperationType == operationType, 
                    cancellationToken: cancellationToken)
                ?? throw new OperationTokenNotFoundException(command.Token, operationType);

            if (!token.IsValid())
            {
                throw new OperationTokenInvalidException(operationType);
            }
            var user = await unitOfWork.User
                .GetAsync(filter: u => u.Id == token.UserId, cancellationToken: cancellationToken)
                ?? throw new UserNotFoundException(token.UserId.Value);

            if (!user.IsConfirmed)
            {
                user.SetConfirmation(true);
                unitOfWork.User.Update(user);
            }

            unitOfWork.OperationToken.Remove(token);
            await unitOfWork.SaveAsync(cancellationToken);

            // return result
            return new ConfirmUserEmailResult(true);
        }
    }
}
