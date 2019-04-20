namespace ClinicApi.Infrastructure.Constants.ValidationErrorMessages
{
    public static class AuthErrorMessages
    {
        public const string FailedAuthorization = "Wrong password or email";
        public const string InvalidRefreshToken = "Bad refresh token";
        public const string InvalidRevokeToken = "Bad revke token";
        public const string CannotRevokeToken = "No access";
    }
}