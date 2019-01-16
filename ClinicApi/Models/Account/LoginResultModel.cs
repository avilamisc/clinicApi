using System;

namespace ClinicApi.Models.Account
{
    public class LoginResultModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
    }
}