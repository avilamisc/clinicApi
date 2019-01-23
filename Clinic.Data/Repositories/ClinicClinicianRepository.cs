using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Context;
using System.Threading.Tasks;
using System.Data.Entity;
using Clinic.Core.DtoModels;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace Clinic.Data.Repositories
{
    public class ClinicClinicianRepository : Repository<ClinicClinician>, IClinicClinicianRepository
    {
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

        public async Task<IEnumerable<ClinicWithDistanceDto>> GetSortedByDistanceClinicClinician(DbGeography distanceFrom)
        {
            var uniqueClinician = _context.ClinicClinicians
                .Include(cc => cc.Clinic)
                .Include(cc => cc.Clinician)
                .Select(cc => new
                {
                    cc.ClinicId,
                    cc.Clinic.City,
                    ClinicName = cc.Clinic.Name,
                    Distance = cc.Clinic.Geolocation.Distance(distanceFrom).Value / 1000,
                    Lat = (double)cc.Clinic.Geolocation.Latitude,
                    Long = (double)cc.Clinic.Geolocation.Longitude,
                    cc.ClinicianId,
                    ClinicianName = cc.Clinician.Name,
                    ClinicianSurname = cc.Clinician.Surname,
                    ClinicianRate = cc.Clinician.Rate
                })
                .GroupBy(cc => cc.ClinicianId)
                .Select(gr => gr.OrderBy(cc => cc.Distance).FirstOrDefault())
                .OrderBy(cc => cc.Distance);

            return await (from gr in uniqueClinician
                          group gr by gr.ClinicId into gr
                          select new ClinicWithDistanceDto
                          {
                              Id = gr.FirstOrDefault().ClinicId,
                              City = gr.FirstOrDefault().City,
                              ClinicName = gr.FirstOrDefault().ClinicName,
                              Lat = gr.FirstOrDefault().Lat,
                              Long = gr.FirstOrDefault().Long,
                              Distance = gr.FirstOrDefault().Distance,
                              Clinicians = gr.Select(item => new ClinicianDto
                              {
                                  Id = item.ClinicianId,
                                  Name = item.ClinicianName,
                                  Rate = item.ClinicianRate,
                                  Surname = item.ClinicianSurname
                              })
                          }).ToListAsync();
        }
    }
}


/*
             var uniqueClinician = _context.ClinicClinicians
                .Include(cc => cc.Clinic)
                .Include(cc => cc.Clinician)
                .Select(cc => new
                {
                    Distance = cc.Clinic.Geolocation.Distance(distanceFrom).Value / 1000,
                    cc.ClinicianId,
                    cc.Clinician,
                    cc.ClinicId
                })
                .GroupBy(cc => cc.ClinicianId)
                .Select(gr => gr.OrderBy(cc => cc.Distance).FirstOrDefault())
                .OrderBy(cc => cc.Distance);

            var t = uniqueClinician.ToList();

            return await (from gr in uniqueClinician
                          group gr by gr.ClinicId into gr
                          from clinic in _context.Clinics
                          where gr.Key == clinic.Id || (uniqueClinician.FirstOrDefault(uc => uc.ClinicId == clinic.Id) == null)
                          select new ClinicWithDistanceDto
                          {
                              Id = clinic.Id,
                              City = clinic.City,
                              ClinicName = clinic.Name,
                              Lat = (double)clinic.Geolocation.Latitude,
                              Long = (double)clinic.Geolocation.Longitude,
                              Distance = clinic.Geolocation.Distance(distanceFrom).Value / 1000,
                              Clinicians = gr != null
                                ? gr.Select(item => new ClinicianDto
                                    {
                                        Id = item.ClinicianId,
                                        Name = item.Clinician.Name,
                                        Rate = item.Clinician.Rate,
                                        Surname = item.Clinician.Surname
                                    })
                                : new List<ClinicianDto>()
                          }).ToListAsync();*/
