﻿using UsersApplication.Interfaces.ServiceContracts;

namespace UsersApplication.Users.Commands.SeekPasswordReset
{
    public class SeekPasswordResetHandler(IUnitOfWork unitOfWork, IEmailService emailService)
        : ICommandHandler<SeekPasswordResetCommand, SeekPasswordResetResult>
    {
        public async Task<SeekPasswordResetResult> Handle(SeekPasswordResetCommand command, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.User
                .GetAsync(filter: u => u.Email == command.Email, cancellationToken: cancellationToken)
                ?? throw new UserNotFoundException(command.Email);

            await emailService.SendPasswordResetAsync(user, cancellationToken);

            return new SeekPasswordResetResult(true);
        }
    }
}
