using Clinic.Core.DtoModels;
using Clinic.Core.DtoModels.Account;
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

        public async Task<Clinician> CreateClinicianAsync(ClinicianRegistrationDto registrationDto)
        {
            var clinician = new Clinician
            {
                Name = registrationDto.Name,
                Surname = registrationDto.Surname,
                Role = registrationDto.Role,
                Email = registrationDto.Email,
                PasswordHash = registrationDto.PasswordHash,
            };
            var clinicClinicians = registrationDto.RelatedClinics
                .Select(c => new ClinicClinician { ClinicId = c.Id, Clinician = clinician });

            _context.ClinicClinicians.AddRange(clinicClinicians);
            _context.Clinicians.Add(clinician);
            await _context.SaveChangesAsync();

            return clinician;
        }

        public async Task<IEnumerable<ClinicianDto>> GetCliniciansAsync(int? clinicId)
        {
            var result = await _context.ClinicClinicians
                    .Include(cc => cc.Clinician)
                    .Where(cc => cc.ClinicId == clinicId || clinicId == null)
                    .Select(cc => cc.Clinician)
                    .ToListAsync();

            return _mapper.Mapper.Map<IEnumerable<ClinicianDto>>(result);
        }
    }
}
