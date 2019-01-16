using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Context;
using Clinic.Domain.Repositories.Base;
using System.Threading.Tasks;

namespace Clinic.Domain.Repositories.Concrete
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
    }
}
