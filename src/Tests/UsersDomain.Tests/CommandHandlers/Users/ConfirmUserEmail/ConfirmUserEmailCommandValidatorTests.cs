using UsersApplication.Users.Commands.ConfirmUserEmail;
using UsersDomain.Errors;

namespace UsersApplication.Tests.CommandHandlers.Users.ConfirmUserEmail
{
    public class ConfirmUserEmailCommandValidatorTests :
        ValidationTestBase<ConfirmUserEmailCommand, ConfirmUserEmailResult, ConfirmUserEmailCommandValidator>
    {
        public ConfirmUserEmailCommandValidatorTests() : base(new ConfirmUserEmailCommandValidator())
        {
        }

        [Fact]
        public void Should_Be_Valid()
        {
            var command = CreateTestCommand();

            ShouldBeValid(command);
        }

        [Fact]
        public void Should_Be_Invalid_For_Empty_Token()
        {
            var command = CreateTestCommand(token: Guid.Empty);

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.OperationToken.EmptyCode,
                "Token may not be empty");
        }

        private static ConfirmUserEmailCommand CreateTestCommand(Guid? token = null)
        {
            return new ConfirmUserEmailCommand(token ?? new Guid("3e5af1a9-aa98-44e5-aadc-6b51dbdbc4c1"));
        }
    }
}
