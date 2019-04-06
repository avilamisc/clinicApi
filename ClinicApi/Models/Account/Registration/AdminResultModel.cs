namespace ClinicApi.Models.Account.Registration
{
    public class AdminRegisterModel : UserRegisterModel
    {
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string City { get; set; }
    }
}