using UsersApplication.Dtos;
using UsersApplication.Users.Commands.ConfirmUserEmail;
using UsersApplication.Users.Commands.ResetPassword;
using UsersDomain.Constants;
using UsersDomain.Enums;
using UsersDomain.Errors;

namespace UsersApplication.Tests.CommandHandlers.Users.ResetPassword
{
    public class ResetPasswordCommandValidatorTests :
        ValidationTestBase<ResetPasswordCommand, ResetPasswordResult, ResetPasswordCommandValidator>
    {
        public ResetPasswordCommandValidatorTests() : base(new ResetPasswordCommandValidator())
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

        [Fact]
        public void Should_Be_Invalid_For_Empty_NewPassword()
        {
            var command = CreateTestCommand(newPassword: "");

            var errors = new List<string>
            {
                DomainErrorCodes.User.EmptyPassword,
                DomainErrorCodes.User.ShortPassword
            };

            ShouldHaveExpectedErrors(command, errors.ToArray());
        }

        [Fact]
        public void Should_Be_Invalid_For_NewPassword_Too_Short()
        {
            var command = CreateTestCommand(newPassword: "zA6{");

            ShouldHaveSingleError(command, DomainErrorCodes.User.ShortPassword);
        }

        [Fact]
        public void Should_Be_Invalid_For_NewPassword_Too_Long()
        {
            var command = CreateTestCommand(newPassword: string.Concat(Enumerable.Repeat("zA6{", 12), 12));

            ShouldHaveSingleError(command, DomainErrorCodes.User.LongPassword);
        }

        private static ResetPasswordCommand CreateTestCommand(
            Guid? token = null,
            string? newPassword = null )
        {
            return new ResetPasswordCommand(
                token ?? new Guid("3e5af1a9-aa98-44e5-aadc-6b51dbdbc4c1"),
                newPassword ?? "Po=PF]PC6t.?8?ks)A6W" );
        }
    }
}
