using System.Web;

namespace ClinicApi.Interfaces
{
    public interface IFileService
    {
        string UploadFile(HttpPostedFile file);
        void DeleteFile(string fullPath);
        byte[] GetFile(string filePath);
    }
}
