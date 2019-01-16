using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Context;
using Clinic.Domain.Repositories.Base;

namespace Clinic.Data.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(ClinicDb context) : base(context)
        {
        }
    }
}
