using System.Threading.Tasks;
using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Context;
using System.Data.Entity;

namespace Clinic.Data.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(
            ClinicDb context) : base(context)
        {
        }

        public async Task<Document> GetWithClinicClinicianByIdAsync(int id)
        {
            return await _context.Documents
                            .Include(d => d.Booking.ClinicClinician)
                            .SingleOrDefaultAsync(d => d.Id == id);
        }
    }
}
