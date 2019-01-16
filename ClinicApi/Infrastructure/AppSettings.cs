namespace ClinicApi.Infrastructure
{
    public class AppSettings
    {
        public string AuthSecret { get; set; }
        public int AccessTokenExpirtionTime { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
        public int WorkFactorComplexity { get; set; }
    }
}