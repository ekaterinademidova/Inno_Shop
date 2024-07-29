using Microsoft.EntityFrameworkCore;

namespace UsersApplication.Users.Commands.DeleteUser
{
    public class DeleteUserHandler(IApplicationDbContext dbContext)
        : ICommandHandler<DeleteUserCommand, DeleteUserResult>
    {
        public async Task<DeleteUserResult> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            // delete User entity from command object
            var userId = UserId.Of(command.UserId);
            var user = await dbContext.Users
                .FindAsync([userId], cancellationToken: cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            // save to database
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            // return result
            return new DeleteUserResult(true);
        }
    }
}
