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

        public async Task<IEnumerable<ClinicWithDistanceDto>> GetClinicClinicianSortedByDistance(DbGeography distanceFrom)
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
                              Distance = gr.FirstOrDefault().Distance / 1000,
                              Clinicians = gr.Select(item => new ClinicianDto
                              {
                                  Id = item.ClinicianId,
                                  Name = item.ClinicianName,
                                  Rate = item.ClinicianRate,
                                  Surname = item.ClinicianSurname
                              })
                          }).ToListAsync();
        }

        public async Task<IEnumerable<ClinicWithDistanceDto>> GetClinicClinicianSortedByDistanceV2(DbGeography location)
        {
            var query = _context.Clinicians
                .Where(c => c.ClinicClinicians.Count() > 0)
                .Select(c => new
                {
                    ClinicianId = c.Id,
                    ClinicianName = c.Name,
                    ClinicianRate = c.Rate,
                    ClinicianSurname = c.Surname,
                    SortedClinic = c.ClinicClinicians.Select(cc => new
                    {
                        cc.Clinic.City,
                        cc.Clinic.Name,
                        cc.Clinic.Id,
                        Distance = cc.Clinic.Geolocation.Distance(location).Value,
                        Lat = cc.Clinic.Geolocation.Latitude,
                        Long = cc.Clinic.Geolocation.Longitude
                    })
                    .OrderBy(cs => cs.Distance)
                    .FirstOrDefault()
                })
                .GroupBy(c => c.SortedClinic.Id)
                .Select(group => new { Key = group.FirstOrDefault(), Values = group })
                .Select(cc => new ClinicWithDistanceDto
                {
                    City = cc.Key.SortedClinic.City,
                    ClinicName = cc.Key.SortedClinic.Name,
                    Distance = cc.Key.SortedClinic.Distance,
                    Id = cc.Key.SortedClinic.Id,
                    Lat = cc.Key.SortedClinic.Lat.Value,
                    Long = cc.Key.SortedClinic.Long.Value,
                    Clinicians = cc.Values.Select(value => new ClinicianDto
                    {
                        Id = value.ClinicianId,
                        Surname = value.ClinicianSurname,
                        Name = value.ClinicianName,
                        Rate = value.ClinicianRate
                    })
                })
                .OrderBy(dto => dto.Distance);

            return await query.ToListAsync();
        }


        public async Task<IEnumerable<ClinicWithDistanceDto>> GetClinicClinicianSortedByDistanceV3(DbGeography location)
        {
            return await _context.Clinics
                .GroupJoin(_context.ClinicClinicians
                                   .GroupBy(cc => cc.ClinicianId)
                                   .Select(gr => gr.OrderBy(grList => grList.Clinic.Geolocation.Distance(location)).FirstOrDefault()),
                           c => c.Id,
                           cc => cc.ClinicId,
                           (c, cc) => new ClinicWithDistanceDto
                           {
                               City = c.City,
                               Id = c.Id,
                               Long = c.Geolocation.Longitude.Value,
                               Lat = c.Geolocation.Latitude.Value,
                               ClinicName = c.Name,
                               Distance = c.Geolocation.Distance(location).Value,
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

