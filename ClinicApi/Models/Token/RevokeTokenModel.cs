namespace ClinicApi.Models.Token
{
    public class RevokeTokenModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}