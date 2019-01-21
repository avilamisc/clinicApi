using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Automapper.Infrastructure;
using Clinic.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<IEnumerable<BookingDto>> GetForClinicianAsync(PagingDto pagingDto, int clinicianId)
        {
            var result = await _context.Bookings
                .OrderBy(b => b.Id)
                .Include(b => b.ClinicClinician.Clinic)
                .Include(b => b.Patient)
                .Include(b => b.Documents)
                .Where(b => b.ClinicClinician.ClinicianId == clinicianId)
                .Skip(pagingDto.PageNumber * pagingDto.PageSize)
                .Take(pagingDto.PageSize)
                .ToListAsync();

            return _mapper.Mapper.Map<List<BookingDto>>(result);
        }

        public async Task<IEnumerable<BookingDto>> GetForPatientAsync(PagingDto pagingDto, int patinetId)
        {
            var result = await _context.Bookings
                .OrderBy(b => b.Id)
                .Include(b => b.ClinicClinician.Clinic)
                .Include(b => b.ClinicClinician.Clinician)
                .Include(b => b.Documents)
                .Where(b => b.PatientId == patinetId)
                .Skip(pagingDto.PageNumber * pagingDto.PageSize)
                .Take(pagingDto.PageSize)
                .ToListAsync();

            return _mapper.Mapper.Map<List<BookingDto>>(result);
        }

        public int CountForPatien(int patinetId)
        {
            return _context.Bookings
                .Where(b => b.PatientId == patinetId).Count();
        }

        public int CountForClinician(int clinicianId)
        {
            return _context.Bookings
                .Include(b => b.ClinicClinician)
                .Where(b => b.ClinicClinician.ClinicianId == clinicianId).Count();
        }

        public async Task<Booking> GetWithDocumentsAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Documents)
                .SingleOrDefaultAsync(b => b.Id == id);
        }
    }
}