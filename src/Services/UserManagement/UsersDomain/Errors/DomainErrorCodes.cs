namespace UsersDomain.Errors;

public static class DomainErrorCodes
{
    public static class User
    {
        // User Validation
        public const string EmptyId = "USER_EMPTY_ID";

        public const string EmptyFirstName = "USER_EMPTY_FIRST_NAME";
        public const string FirstNameExceedsMaxLength = "USER_FIRST_NAME_EXCEEDS_MAX_LENGTH";

        public const string EmptyLastName = "USER_EMPTY_LAST_NAME";
        public const string LastNameExceedsMaxLength = "USER_LAST_NAME_EXCEEDS_MAX_LENGTH";

        public const string InvalidEmail = "USER_INVALID_EMAIL";
        public const string EmailExceedsMaxLength = "USER_EMAIL_EXCEEDS_MAX_LENGTH";
        public const string EmailIsNotConfirmed = "USER_EMAIL_IS_NOT_CONFIRMED";

        public const string EmptyPassword = "USER_PASSWORD_MAY_NOT_BE_EMPTY";
        public const string ShortPassword = "USER_PASSWORD_MAY_NOT_BE_SHORTER_THAN_8_CHARACTERS";
        public const string LongPassword = "USER_PASSWORD_MAY_NOT_BE_LONGER_THAN_50_CHARACTERS";

        public const string InvalidRole = "USER_INVALID_ROLE";

        // General
        public const string AlreadyExists = "USER_ALREADY_EXISTS";
    }

    public static class OperationToken
    {
        // OperationToken Validation
        public const string EmptyId = "OPERATION_TOKEN_EMPTY_ID";
        public const string EmptyUserId = "OPERATION_TOKEN_EMPTY_USER_ID";
        public const string EmptyCode = "OPERATION_TOKEN_EMPTY_CODE";
        public const string EmptyOperationType = "OPERATION_TOKEN_EMPTY_OPERATION_TYPE";
        public const string EmptyExpiration = "OPERATION_TOKEN_EMPTY_EXPIRATION";
        public const string InvalidToken = "OPERATION_TOKEN_IS_INVALID";

        // General
        public const string AlreadyExists = "OPERATION_TOKEN_ALREADY_EXISTS";
    }

    public static class Product
    {
        // Product Validation
        public const string EmptyId = "PRODUCT_EMPTY_ID";
        public const string EmptyName = "PRODUCT_EMPTY_NAME";
        public const string ShortName = "PRODUCT_NAME_MAY_NOT_BE_SHORTER_THAN_4_CHARACTERS";
        public const string LongName = "PRODUCT_NAME_MAY_NOT_BE_LONGER_THAN_150_CHARACTERS";
        public const string EmptyImageFile = "PRODUCT_EMPTY_IMAGE_FILE";
        public const string PriceLessThanZero = "PRODUCT_PRICE_LESS_THAN_ZERO";

        // General
        public const string AlreadyExists = "PRODUCT_ALREADY_EXISTS";
    }
}