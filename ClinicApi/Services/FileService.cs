using System;
using System.IO;
using System.Web;
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

        public string UploadFile(HttpPostedFile file)
        {
            if (file == null) return null;

            var ext = Path.GetExtension(file.FileName);
            var fileName = Path.GetFileName($"{file.FileName}_{Guid.NewGuid().ToString()}{ext}");
            var filePath = Path.Combine(ApiConstants.UploadedFilesFolderPath, fileName);

            file.SaveAs(filePath); 

            return filePath;
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