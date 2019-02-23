using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Context;
using Clinic.Core.DtoModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Data.Repositories
{
    public class ClinicClinicianRepository : Repository<ClinicClinician>, IClinicClinicianRepository
    {
        private const int MetersPerKilometer = 1000;

        public ClinicClinicianRepository(ClinicDb context) : base(context)
        {
        }

        public async Task<ClinicClinician> GetClinicClinicianAsync(int clinicId, int clinicianId)
        {
            return await _context.ClinicClinicians
                .Include(c => c.Clinic)
                .Include(c => c.Clinician)
                .SingleOrDefaultAsync(c => c.ClinicId == clinicId && c.ClinicianId == clinicianId);
        }

        public async Task<IEnumerable<ClinicLocationDto>> GetClinicsWithClinicianSortedByDistanceAsync_V1(
            DbGeography distanceFrom)
        {
            var uniqueClinician = _context.ClinicClinicians
                .Include(cc => cc.Clinic)
                .Include(cc => cc.Clinician)
                .Select(cc => new
                {
                    cc.ClinicId,
                    cc.Clinic.City,
                    ClinicName = cc.Clinic.Name,
                    Distance = cc.Clinic.Geolocation.Distance(distanceFrom).Value,
                    Lat = (double)cc.Clinic.Geolocation.Latitude,
                    Long = (double)cc.Clinic.Geolocation.Longitude,
                    cc.ClinicianId,
                    ClinicianName = cc.Clinician.Name,
                    ClinicianSurname = cc.Clinician.Surname,
                    ClinicianRate = cc.Clinician.Rate
                })
                .GroupBy(cc => cc.ClinicianId)
                .Select(gr => gr.OrderBy(cc => cc.Distance).FirstOrDefault());

            return await (from gr in uniqueClinician
                          group gr by gr.ClinicId into gr
                          select new ClinicLocationDto
                          {
                              Id = gr.FirstOrDefault().ClinicId,
                              City = gr.FirstOrDefault().City,
                              ClinicName = gr.FirstOrDefault().ClinicName,
                              Lat = gr.FirstOrDefault().Lat,
                              Long = gr.FirstOrDefault().Long,
                              Distance = gr.FirstOrDefault().Distance / MetersPerKilometer,
                              Clinicians = gr.Select(item => new ClinicianDto
                              {
                                  Id = item.ClinicianId,
                                  Name = item.ClinicianName,
                                  Rate = item.ClinicianRate,
                                  Surname = item.ClinicianSurname
                              })
                          })
                          .OrderBy(c => c.Distance)
                          .ToListAsync();
        }

        public async Task<IEnumerable<ClinicLocationDto>> GetClinicsWithClinicianSortedByDistanceAsync_V2(
            DbGeography location)
        {
            var result = await _context.ClinicClinicians
                .GroupBy(cc => cc.ClinicianId)
                .Select(gr => gr.OrderBy(grList => grList.Clinic.Geolocation.Distance(location)).FirstOrDefault())
                .GroupBy(cc => cc.ClinicId)
                .Select(gr => new
                {
                    gr.FirstOrDefault().Clinic,
                    Clinicians = gr.Select(cc => cc.Clinician)
                })
                .Select(cc => new ClinicLocationDto
                {
                    City = cc.Clinic.City,
                    ClinicName = cc.Clinic.Name,
                    Lat = cc.Clinic.Geolocation.Latitude.Value,
                    Long = cc.Clinic.Geolocation.Longitude.Value,
                    Id = cc.Clinic.Id,
                    Distance = cc.Clinic.Geolocation.Distance(location).Value / MetersPerKilometer,
                    Clinicians = cc.Clinicians.Select(c => new ClinicianDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Rate = c.Rate,
                        Surname = c.Surname
                    })
                })
                .OrderBy(dto => dto.Distance)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<ClinicLocationDto>> GetClinicsWithClinicianSortedByDistanceAsync_V3(
            DbGeography location)
        {
            return await _context.Clinics
                .GroupJoin(_context.ClinicClinicians
                .Select(cc => new {
                    ClinicId = cc.ClinicId,
                    Clinician = cc.Clinician,
                    Distance = cc.Clinic.Geolocation.Distance(location)
                })
                .GroupBy(cc => cc.Clinician.Id)
                .Select(gr => gr.OrderBy(grList => grList.Distance).FirstOrDefault()),
                           c => c.Id,
                           cc => cc.ClinicId,
                           (c, cc) => new ClinicLocationDto
                           {
                               City = c.City,
                               Id = c.Id,
                               Long = c.Geolocation.Longitude.Value,
                               Lat = c.Geolocation.Latitude.Value,
                               ClinicName = c.Name,
                               Distance = c.Geolocation.Distance(location).Value / MetersPerKilometer,
                               Clinicians = cc.Select(cl => new ClinicianDto
                               {
                                   Id = cl.Clinician.Id,
                                   Name = cl.Clinician.Name,
                                   Rate = cl.Clinician.Rate,
                                   Surname = cl.Clinician.Surname
                               })
                           })
                .OrderBy(c => c.Distance)
                .ToListAsync();
        }

    }
}

