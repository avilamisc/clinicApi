using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Core.Repositories;
using Clinic.Data.Automapper.Infrastructure;
using Clinic.Data.Common;
using Clinic.Data.Context;


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

        public async Task<PagingResultDto<BookingDto>> GetForClinicianAsync(
            PagingDto pagingDto, int clinicianId, Stage? stage)
        {
            var result = await _context.Bookings
                .BookingInclude()
                .Include(b => b.Patient)
                .Where(b => b.ClinicClinician.ClinicianId == clinicianId &&
                            (!stage.HasValue || b.Stage == stage.Value))
                .Paging(pagingDto)
                .ToListAsync();

            var totalCount = _context.Bookings
                .Include(b => b.ClinicClinician)
                .Where(b => b.ClinicClinician.ClinicianId == clinicianId &&
                            (!stage.HasValue || b.Stage == stage.Value)).Count();

            return new PagingResultDto<BookingDto>
                    {
                        DataColection = _mapper.Mapper.Map<List<BookingDto>>(result),
                        TotalCount = totalCount
                    };
        }

        public async Task<PagingResultDto<BookingDto>> GetForPatientAsync(
            PagingDto pagingDto, int patinetId, Stage? stage)
        {
            var result = await _context.Bookings
                .BookingInclude()
                .Include(b => b.ClinicClinician.Clinician)
                .Where(b => b.PatientId == patinetId &&
                            (!stage.HasValue || b.Stage == stage.Value))
                .Paging(pagingDto)
                .ToListAsync();

            var totalCount = _context.Bookings
                .Where(b => b.PatientId == patinetId &&
                            (!stage.HasValue || b.Stage == stage.Value)).Count();

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

        public void UpdateWithRecalculatingRateAsync(Booking entity) {
            _context.Entry(entity).Reference(e => e.ClinicClinician).Load();
            var clinician = _context.Clinicians.Find(entity.ClinicClinician.ClinicianId);
            clinician.Rate = GetNewClinicianRateOnUpdateBooking(entity.ClinicClinician.ClinicianId, entity);

            _context.Entry(entity).State = EntityState.Modified;
            _context.Entry(clinician).State = EntityState.Modified;
        }

        public float GetClinicianRateAsync(int clinicianId)
        {
            int countBooking = _context.Bookings
                .Where(b => b.Rate != null && b.ClinicClinician.ClinicianId == clinicianId)
                .Count();

            if (countBooking == 0) return 0;

            float totalRate = countBooking > 0
                ? _context.Bookings
                          .Where(b => b.Rate != null && b.ClinicClinician.ClinicianId == clinicianId)
                          .Sum(b => b.Rate ?? 0)
                : 0;

            return totalRate / countBooking;
        }

        private float GetNewClinicianRateOnUpdateBooking(int clinicianId, Booking booking)
        {
            int countBooking = _context.Bookings
                .Where(b => b.Rate != null &&
                       b.ClinicClinician.ClinicianId == clinicianId &&
                       b.Id != booking.Id)
                .Count();

            if (countBooking == 0)
            {
                return booking.Rate ?? 0;
            }

            float totalRate = countBooking > 0
                ? _context.Bookings
                    .Where(b => b.Rate.HasValue && 
                        b.ClinicClinician.ClinicianId == clinicianId &&
                        b.Id != booking.Id)
                    .Sum(b => b.Rate.Value)
                : 0;

            return ((booking.Rate ?? 0) + totalRate) / countBooking;
        }
    }
}