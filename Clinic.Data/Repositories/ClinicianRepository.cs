using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Automapper.Infrastructure;
using Clinic.Data.Context;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Data.Repositories
{
    public class ClinicianRepository: Repository<Clinician>, IClinicianRepository
    {
        private readonly IDataMapper _mapper;

        public ClinicianRepository(
            IDataMapper mapper,
            ClinicDb clinicDb) : base(clinicDb)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClinicianDto>> GetCliniciansAsync(int clinicId = -1)
        {
            var query = clinicId == -1
                ? _context.Clinicians
                : _context.ClinicClinicians
                    .Include(cc => cc.Clinician)
                    .Where(cc => cc.ClinicId == clinicId)
                    .Select(cc => cc.Clinician);

            return _mapper.Mapper.Map<IEnumerable<ClinicianDto>>(await query.ToListAsync());
        }
    }
}
