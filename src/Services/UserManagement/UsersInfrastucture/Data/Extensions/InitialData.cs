using UsersDomain.Constants;
using UsersDomain.Enums;
using UsersDomain.ValueObjects;

namespace UsersInfrastructure.Data.Extensions
{
    public class InitialData
    {
        public static IEnumerable<User> Users
        {
            get
            {
                var user0 = User.Create(
                                UserData.Ids.AdminId,
                                "UserFirstName 0",
                                "UserLastName 0",
                                UserData.Emails.AdminEmail,
                                UserData.Passwords.AdminPassword);
                user0.SetRole(UserRole.Admin);

                var user1 = User.Create(
                                UserData.Ids.User1Id,
                                "UserFirstName 1",
                                "UserLastName 1",
                                UserData.Emails.User1Email,
                                UserData.Passwords.User1Password);

                var user2 = User.Create(
                                UserData.Ids.User2Id,
                                "UserFirstName 2",
                                "UserLastName 2",
                                UserData.Emails.User2Email,
                                UserData.Passwords.User2Password);

                var user3 = User.Create(
                                UserData.Ids.User3Id,
                                "UserFirstName 3",
                                "UserLastName 3",
                                UserData.Emails.User3Email,
                                UserData.Passwords.User3Password);

                return [user0, user1, user2, user3];
            }
        }

        public static IEnumerable<OperationToken> OperationTokens
        {
            get
            {
                var emailToken0 = OperationToken.Create(UserData.Ids.AdminId, OperationTokenData.Codes.AdminEmailConfirmationCode, OperationType.EmailConfirmation, 3600000);
                var emailToken1 = OperationToken.Create(UserData.Ids.User1Id, OperationTokenData.Codes.User1EmailConfirmationCode, OperationType.EmailConfirmation, 3600000);
                var emailToken2 = OperationToken.Create(UserData.Ids.User2Id, OperationTokenData.Codes.User2EmailConfirmationCode, OperationType.EmailConfirmation, 3600000);
                var emailToken3 = OperationToken.Create(UserData.Ids.User3Id, OperationTokenData.Codes.User3EmailConfirmationCode, OperationType.EmailConfirmation, 3600000);

                var resetPasswordToken0 = OperationToken.Create(UserData.Ids.AdminId, OperationTokenData.Codes.AdminPasswordResetCode, OperationType.PasswordReset);
                var resetPasswordToken1 = OperationToken.Create(UserData.Ids.User1Id, OperationTokenData.Codes.User1PasswordResetCode, OperationType.PasswordReset);
                var resetPasswordToken2 = OperationToken.Create(UserData.Ids.User2Id, OperationTokenData.Codes.User2PasswordResetCode, OperationType.PasswordReset);
                var resetPasswordToken3 = OperationToken.Create(UserData.Ids.User3Id, OperationTokenData.Codes.User3PasswordResetCode, OperationType.PasswordReset);

                return [emailToken0, emailToken1, emailToken2, emailToken3, 
                    resetPasswordToken0, resetPasswordToken1, resetPasswordToken2, resetPasswordToken3];
            }
        }
    }
}
