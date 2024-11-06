using UsersApplication.Dtos;
using UsersApplication.Users.Commands.CreateUser;
using UsersDomain.Constants;
using UsersDomain.Enums;
using UsersDomain.Errors;

namespace UsersApplication.Tests.CommandHandlers.Users.CreateUser
{
    public class CreateUserCommandValidatorTests :
        ValidationTestBase<CreateUserCommand, CreateUserResult, CreateUserCommandValidator>
    {
        public CreateUserCommandValidatorTests() : base(new CreateUserCommandValidator())
        {
        }

        [Fact]
        public void Should_Be_Valid()
        {
            var command = CreateTestCommand();

            ShouldBeValid(command);
        }

        [Fact]
        public void Should_Be_Invalid_For_Empty_First_Name()
        {
            var command = CreateTestCommand(firstName: "");

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.User.EmptyFirstName,
                "FirstName may not be empty");
        }

        [Fact]
        public void Should_Be_Invalid_For_First_Name_Exceeds_Max_Length()
        {
            var command = CreateTestCommand(firstName: new string('a', MaxLengths.User.FirstName + 1));

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.User.FirstNameExceedsMaxLength,
                $"FirstName may not be longer than {MaxLengths.User.FirstName} characters");
        }

        [Fact]
        public void Should_Be_Invalid_For_Empty_Last_Name()
        {
            var command = CreateTestCommand(lastName: "");

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.User.EmptyLastName,
                "LastName may not be empty");
        }

        [Fact]
        public void Should_Be_Invalid_For_Last_Name_Exceeds_Max_Length()
        {
            var command = CreateTestCommand(lastName: new string('a', MaxLengths.User.LastName + 1));

            ShouldHaveSingleError(
                command,
                DomainErrorCodes.User.LastNameExceedsMaxLength,
                $"LastName may not be longer than {MaxLengths.User.LastName} characters");
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

        private static CreateUserCommand CreateTestCommand(
            string? firstName = null,
            string? lastName = null,
            string? email = null,
            string? password = null)//,
            //UserRole? role = null )
        {
            return new CreateUserCommand(new UserDto
            {
                FirstName = firstName ?? "TestFirstName",
                LastName = lastName ?? "TestLastName",
                Email = email ?? "test.email@gmail.com",
                Password = password ?? "Po=PF]PC6t.?8?ks)A6W"//,
                //Role = role ?? UserRole.User
            });

        }
    }
}
