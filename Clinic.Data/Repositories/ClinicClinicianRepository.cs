using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Context;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Clinic.Data.Repositories
{
    public class ClinicClinicianRepository : Repository<ClinicClinician>, IClinicClinicianRepository
    {
        public ClinicClinicianRepository(ClinicDb context) : base(context)
        {
        }

        public async Task UploadClinicAsync(ClinicClinician entity)
        {
            await _context.Entry(entity).Reference(e => e.Clinic).LoadAsync();
        }

        public async Task UploadClinicianAsync(ClinicClinician entity)
        {
            await _context.Entry(entity).Reference(e => e.Clinician).LoadAsync();
        }

        public async Task<ClinicClinician> GetClinicClinicianAsync(int clinicId, int clinicianId)
        {
            return await _context.ClinicClinicians
                .Include(c => c.Clinic)
                .Include(c => c.Clinician)
                .SingleOrDefaultAsync(c => c.ClinicId == clinicId && c.ClinicianId == clinicianId);
        }
    }
}
