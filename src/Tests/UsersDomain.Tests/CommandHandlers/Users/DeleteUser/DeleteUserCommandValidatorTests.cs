using UsersApplication.Dtos;
using UsersApplication.Users.Commands.DeleteUser;
using UsersApplication.Users.Commands.UpdateUser;
using UsersDomain.Constants;
using UsersDomain.Enums;
using UsersDomain.Errors;

namespace UsersApplication.Tests.CommandHandlers.Users.DeleteUser
{
    public class DeleteUserCommandValidatorTests :
        ValidationTestBase<DeleteUserCommand, DeleteUserResult, DeleteUserCommandValidator>
    {
        public DeleteUserCommandValidatorTests() : base(new DeleteUserCommandValidator())
        {
        }

        [Fact]
        public void Should_Be_Valid()
        {
            var command = CreateTestCommand();

            ShouldBeValid(command);
        }

        [Fact]
        public void Should_Be_Invalid_For_Empty_Id()
        {
            var command = CreateTestCommand(id: Guid.Empty);

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.User.EmptyId,
                "User id may not be empty");
        }

        private static DeleteUserCommand CreateTestCommand(Guid? id = null)
        {
            return new DeleteUserCommand(id ?? new Guid("3e5af1a9-aa98-44e5-aadc-6b51dbdbc4c1"));
        }
    }
}
