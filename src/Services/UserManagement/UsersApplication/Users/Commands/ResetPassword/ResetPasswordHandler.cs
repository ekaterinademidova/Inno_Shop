using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersApplication.Users.Commands.ConfirmUserEmail;

namespace UsersApplication.Users.Commands.ResetPassword
{
    public class ResetPasswordHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<ResetPasswordCommand, ResetPasswordResult>
    {
        public async Task<ResetPasswordResult> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var operationType = OperationType.PasswordReset;

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

            user.SetPassword(command.NewPassword);
            unitOfWork.User.Update(user);

            unitOfWork.OperationToken.Remove(token);
            await unitOfWork.SaveAsync(cancellationToken);

            // return result
            return new ResetPasswordResult(true);
        }
    }
}
