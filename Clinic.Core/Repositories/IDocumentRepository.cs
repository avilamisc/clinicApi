using Clinic.Core.Entities;
using System.Threading.Tasks;

namespace Clinic.Core.Repositories
{
    public interface IDocumentRepository : IRepository<Document>
    {
        Task<Document> GetWithClinicClinicianByIdAsync(int id);
    }
}
