using System.Web.Hosting;

namespace ClinicApi.Infrastructure.Constants
{
    public static class ApiConstants
    {
        public const string UserIdClaimName = "UserID";
        public const string ConnectionStirngName = "ClinicDb";
        public static readonly string UploadedFilesFolderPath;

        static ApiConstants()
        {
            UploadedFilesFolderPath = HostingEnvironment.MapPath(@"\App_Data\uploads");
        }
    }
}