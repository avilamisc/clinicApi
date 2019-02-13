using ClinicApi.Infrastructure.Auth;
using ClinicApi.Interfaces;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ClinicApi.Controllers
{
    [RoutePrefix("api/documents")]
    [BearerAuthorization]
    public class DocumentController : ApiController
    {
        private readonly IFileService _fileService;
        private readonly IDocumentService _documentService;

        public DocumentController(
            IDocumentService documentService,
            IFileService fileService)
        {
            _documentService = documentService;
            _fileService = fileService;
        }

        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> Documents(int id)
        {
            var identity = (ClaimsIdentity)User.Identity;

            var document = await _documentService.GetDocumentByIdAsync(identity.Claims, id);
            if (document == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            var fileBytes = _fileService.GetFile(document.FilePath);
            if (fileBytes == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(fileBytes);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(document.FilePath));

            return response;
        }
    }
}