using System;
using System.Threading.Tasks;

using Clinic.Core.DtoModels.Account;
using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Context;

namespace Clinic.Data.Repositories
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(ClinicDb context) : base(context)
        {
        }

        public async Task<Patient> CreatePatientAsync(PatientRegistrationDto registrationDto)
        {
            var patient = new Patient
            {
                Name = registrationDto.Name,
                Surname = registrationDto.Surname,
                Role = registrationDto.Role,
                Email = registrationDto.Email,
                PasswordHash = registrationDto.PasswordHash,
                BornDate = registrationDto.BornDate,
                RegistrationDate = DateTime.Now,
                ImageUrl = registrationDto.UserImage
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return patient;
        }
    }
}
