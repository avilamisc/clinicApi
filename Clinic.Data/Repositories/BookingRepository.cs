using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Automapper.Infrastructure;
using Clinic.Data.Common;
using Clinic.Data.Context;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Data.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly IDataMapper _mapper;

        public BookingRepository(
            IDataMapper mapper,
            ClinicDb context) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<PagingResultDto<BookingDto>> GetForClinicianAsync(PagingDto pagingDto, int clinicianId)
        {
            DbGeography g = DbGeography.FromText($"POINT({10} {10})");
            var result = await _context.Bookings
                .BookingInclude()
                .Include(b => b.Patient)
                .Where(b => b.ClinicClinician.ClinicianId == clinicianId)
                .Paging(pagingDto)
                .ToListAsync();

            var totalCount = _context.Bookings
                .Include(b => b.ClinicClinician)
                .Where(b => b.ClinicClinician.ClinicianId == clinicianId).Count();

            return new PagingResultDto<BookingDto>
                    {
                        DataColection = _mapper.Mapper.Map<List<BookingDto>>(result),
                        TotalCount = totalCount
                    };
        }

        public async Task<PagingResultDto<BookingDto>> GetForPatientAsync(PagingDto pagingDto, int patinetId)
        {
            var result = await _context.Bookings
                .BookingInclude()
                .Include(b => b.ClinicClinician.Clinician)
                .Where(b => b.PatientId == patinetId)
                .Paging(pagingDto)
                .ToListAsync();

            var totalCount = _context.Bookings
                .Where(b => b.PatientId == patinetId).Count();

            return new PagingResultDto<BookingDto>
                    {
                        DataColection = _mapper.Mapper.Map<List<BookingDto>>(result),
                        TotalCount = totalCount
                    };
        }

        public async Task<Booking> GetWithDocumentsAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Documents)
                .SingleOrDefaultAsync(b => b.Id == id);
        }
    }
}