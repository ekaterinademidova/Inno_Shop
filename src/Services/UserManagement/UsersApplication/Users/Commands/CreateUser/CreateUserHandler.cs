﻿namespace UsersApplication.Users.Commands.CreateUser
{
    public class CreateUserHandler(IUnitOfWork unitOfWork)
        : ICommandHandler<CreateUserCommand, CreateUserResult>
    {
        public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await unitOfWork.User
                .GetAsync(filter: u => u.Email == command.User.Email, cancellationToken: cancellationToken);
                
            if (existingUser is not null) 
                throw new UserInvalidDataException($"The user with email \"{command.User.Email})\" already exists.");

            var user = CreateNewUser(command.User);

            await unitOfWork.User.AddAsync(user, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);

            return new CreateUserResult(user.Id.Value);
        }

        private static User CreateNewUser(UserDto userDto)
        {
            var newUser = User.Create(
                id: UserId.Of(Guid.NewGuid()),
                firstName: userDto.FirstName,
                lastName: userDto.LastName,
                email: userDto.Email,
                password: userDto.Password
            );

            return newUser;
        }
    }
}
