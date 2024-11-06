using UsersApplication.Users.Commands.LoginUser;
using UsersDomain.Constants;
using UsersDomain.Errors;

namespace UsersApplication.Tests.CommandHandlers.Users.LoginUser
{
    public class LoginUserCommandValidatorTests :
        ValidationTestBase<LoginUserCommand, LoginUserResult, LoginUserCommandValidator>
    {
        public LoginUserCommandValidatorTests() : base(new LoginUserCommandValidator())
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

        [Fact]
        public void Should_Be_Invalid_For_Empty_Password()
        {
            var command = CreateTestCommand(password: "");

            var errors = new List<string>
            {
                DomainErrorCodes.User.EmptyPassword,
                DomainErrorCodes.User.ShortPassword
            };

            ShouldHaveExpectedErrors(command, errors.ToArray());
        }

        [Fact]
        public void Should_Be_Invalid_For_Password_Too_Short()
        {
            var command = CreateTestCommand(password: "zA6{");

            ShouldHaveSingleError(command, DomainErrorCodes.User.ShortPassword);
        }

        [Fact]
        public void Should_Be_Invalid_For_Password_Too_Long()
        {
            var command = CreateTestCommand(password: string.Concat(Enumerable.Repeat("zA6{", 12), 12));

            ShouldHaveSingleError(command, DomainErrorCodes.User.LongPassword);
        }

        private static LoginUserCommand CreateTestCommand(
            string? email = null,
            string? password = null )
        {
            return new LoginUserCommand(
                email ?? "testemail@gmail.com",
                password ?? "Po=PF]PC6t.?8?ks)A6W" );
        }
    }
}
