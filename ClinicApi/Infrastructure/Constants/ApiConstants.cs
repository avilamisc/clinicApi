using System.Web.Hosting;

namespace ClinicApi.Infrastructure.Constants
{
    public static class ApiConstants
    {
        public const string UserIdClaimName = "UserID";
        public const string ConnectionStirngName = "ClinicDb";
        public const string ImageFieldName = "profileImage";
        public const string PatientProfileImagesFolder = @"\App_Data\patientProfileImages";
        public const string ClinicianProfileImagesFolder = @"\App_Data\clinicianProfileImages";
        public const string ClinicProfileImagesFolder = @"\App_Data\cinicProfileImages";

        public static readonly string UploadedFilesFolderPath;

        static ApiConstants()
        {
            UploadedFilesFolderPath = HostingEnvironment.MapPath(@"\App_Data\uploads");
        }
    }
}