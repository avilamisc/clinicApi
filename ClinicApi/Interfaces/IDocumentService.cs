using ClinicApi.Models.Document;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IDocumentService
    {
        Task<DocumentModel> GetDocumentByIdAsync(IEnumerable<Claim> claims, int id);
    }
}
