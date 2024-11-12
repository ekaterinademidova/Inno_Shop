using Microsoft.EntityFrameworkCore;
using UsersApplication.Interfaces;

namespace UsersApplication.Users.Commands.DeleteUser
{
    public class DeleteUserHandler(IUnitOfWork unitOfWork)
        : ICommandHandler<DeleteUserCommand, DeleteUserResult>
    {
        public async Task<DeleteUserResult> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(command.UserId);
            var user = await unitOfWork.User
                .GetAsync(filter: u => u.Id == userId, cancellationToken: cancellationToken)
                ?? throw new UserNotFoundException(command.UserId);

            unitOfWork.User.Remove(user);
            await unitOfWork.SaveAsync(cancellationToken);

            return new DeleteUserResult(true);
        }
    }
}
