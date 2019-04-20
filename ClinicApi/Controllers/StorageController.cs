using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

using ClinicApi.Interfaces;

namespace ClinicApi.Controllers
{
    public class StorageController : ApiController
    {
        private readonly IFileService _fileService;

        public StorageController(IFileService fileService)
        {
            _fileService = fileService;
        }
        
        [HttpGet]
        [Route("api/storage")]
        public HttpResponseMessage GetFile(string path)
        {
            var fileContent = _fileService.GetFile(HostingEnvironment.MapPath(path));
            if (fileContent == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileContent)
            };

            var fileName = System.IO.Path.GetFileName(path);

            result.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            return result;
        }
    }
}
