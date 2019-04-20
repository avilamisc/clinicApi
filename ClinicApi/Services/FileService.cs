using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Routing;

using ClinicApi.Infrastructure.Constants;
using ClinicApi.Interfaces;

namespace ClinicApi.Services
{
    public class FileService : IFileService
    {
        public byte[] GetFile(string filePath)
        {
            if (filePath == null || !File.Exists(filePath)) return null;

            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch
            {
                return null;
            }
        }

        public string GetValidUrl(UrlHelper urlHelper, string path)
        {
            return path == null ? null : urlHelper.Link("DefaultApi", new { Controller = "storage", path });
        }

        public string UploadFile(HttpPostedFile file)
        {
            if (file == null) return null;

            var ext = Path.GetExtension(file.FileName);
            var fileName = Path.GetFileName($"{file.FileName}_{Guid.NewGuid().ToString()}{ext}");
            var filePath = Path.Combine(ApiConstants.UploadedFilesFolderPath, fileName);

            file.SaveAs(filePath); 

            return filePath;
        }

        public string UploadFile(string folderPath, HttpPostedFile file)
        {
            var folder = HostingEnvironment.MapPath(folderPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var fileExtension = file.FileName != "blob"
                ? Path.GetExtension(file.FileName)
                : ".png";
            var fileName = Path.GetFileName($"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{fileExtension}");
            var filePath = Path.Combine(folder, fileName);
            file.SaveAs(filePath);

            return Path.Combine(folderPath, fileName);
        }

        public void DeleteFile(string fullPath)
        {
            if (fullPath != null && File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}