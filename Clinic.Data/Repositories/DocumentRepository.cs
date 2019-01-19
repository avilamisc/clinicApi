using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Context;

namespace Clinic.Data.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(ClinicDb context) : base(context)
        {
        }
    }
}
