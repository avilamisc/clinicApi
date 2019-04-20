using System.Web;
using System.Web.Http.Routing;

namespace ClinicApi.Interfaces
{
    public interface IFileService
    {
        string UploadFile(HttpPostedFile file);
        string UploadFile(string folderPath, HttpPostedFile file);
        void DeleteFile(string fullPath);
        byte[] GetFile(string filePath);
        string GetValidUrl(UrlHelper urlHelper, string path);
    }
}
