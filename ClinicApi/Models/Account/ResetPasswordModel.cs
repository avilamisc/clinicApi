namespace ClinicApi.Models.Account
{
    public class ResetPasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RefreshToken { get; set; }
    }
}