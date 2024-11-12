namespace UsersDomain.Constants
{
    public static class OperationTokenData
    {
        public static class Ids
        {
            public static readonly OperationTokenId AdminEmailConfirmationToken = OperationTokenId.Of(new Guid("ac0e6691-8b4d-45a3-ae2b-582c88ada3bb"));
            public static readonly OperationTokenId User1EmailConfirmationToken = OperationTokenId.Of(new Guid("3e5af1a9-aa98-44e5-aadc-6b51dbdbc4c1"));
            public static readonly OperationTokenId User2EmailConfirmationToken = OperationTokenId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"));
            public static readonly OperationTokenId User3EmailConfirmationToken = OperationTokenId.Of(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"));

            public static readonly OperationTokenId AdminPasswordResetToken = OperationTokenId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"));
            public static readonly OperationTokenId User1PasswordResetToken = OperationTokenId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"));
            public static readonly OperationTokenId User2PasswordResetToken = OperationTokenId.Of(new Guid("b786103d-c621-4f5a-b498-23452610f88c"));
            public static readonly OperationTokenId User3PasswordResetToken = OperationTokenId.Of(new Guid("c4bbc4a2-4555-45d8-97cc-2a99b2167bff"));
        }

        public static class Codes
        {
            public static readonly Guid AdminEmailConfirmationCode = new Guid("ac0e6691-8b4d-45a3-ae2b-582c88ada3bb");
            public static readonly Guid User1EmailConfirmationCode = new Guid("3e5af1a9-aa98-44e5-aadc-6b51dbdbc4c1");
            public static readonly Guid User2EmailConfirmationCode = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61");
            public static readonly Guid User3EmailConfirmationCode = new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914");

            public static readonly Guid AdminPasswordResetCode = new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8");
            public static readonly Guid User1PasswordResetCode = new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27");
            public static readonly Guid User2PasswordResetCode = new Guid("b786103d-c621-4f5a-b498-23452610f88c");
            public static readonly Guid User3PasswordResetCode = new Guid("c4bbc4a2-4555-45d8-97cc-2a99b2167bff");
        }
    }
}