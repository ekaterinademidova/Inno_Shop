using UsersApplication.Users.Commands.SeekPasswordReset;
using UsersDomain.Constants;
using UsersDomain.Errors;

namespace UsersApplication.Tests.CommandHandlers.Users.SeekPasswordReset
{
    public class SeekPasswordResetCommandValidatorTests :
        ValidationTestBase<SeekPasswordResetCommand, SeekPasswordResetResult, SeekPasswordResetCommandValidator>
    {
        public SeekPasswordResetCommandValidatorTests() : base(new SeekPasswordResetCommandValidator())
        {
        }

        [Fact]
        public void Should_Be_Valid()
        {
            var command = CreateTestCommand();

            ShouldBeValid(command);
        }

        [Fact]
        public void Should_Be_Invalid_For_Empty_Email()
        {
            var command = CreateTestCommand(email: string.Empty);

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.User.InvalidEmail,
                "Email is not a valid email address");
        }

        [Fact]
        public void Should_Be_Invalid_For_Invalid_Email()
        {
            var command = CreateTestCommand(email: "not an email");

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.User.InvalidEmail,
                "Email is not a valid email address");
        }

        [Fact]
        public void Should_Be_Invalid_For_Email_Exceeds_Max_Length()
        {
            var command = CreateTestCommand(email: new string('a', MaxLengths.User.Email) + "@test.com");

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.User.EmailExceedsMaxLength,
                $"Email may not be longer than {MaxLengths.User.Email} characters");
        }

        private static SeekPasswordResetCommand CreateTestCommand(string? email = null )
        {
            return new SeekPasswordResetCommand(email ?? "testemail@gmail.com");
        }
    }
}
