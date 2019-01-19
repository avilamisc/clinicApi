using System.Threading.Tasks;
using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Context;
using System.Data.Entity;
using Clinic.Data.Automapper.Infrastructure;

namespace Clinic.Data.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(
            ClinicDb context) : base(context)
        {
        }
    }
}
