using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Automapper.Infrastructure;
using Clinic.Data.Context;
using Clinic.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Clinic.Domain.Repositories.Concrete
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

        public async Task<IEnumerable<BookingDto>> GetForClinicianAsync(Expression<Func<Booking, bool>> predicate)
        {
            var result = await _context.Bookings
                .Include(b => b.ClinicClinician.Clinic)
                .Include(b => b.Patient)
                .Include(b => b.Documents)
                .Where(predicate)
                .ToListAsync();

            return _mapper.Mapper.Map<List<BookingDto>>(result);
        }

        public async Task<IEnumerable<BookingDto>> GetForPatientAsync(Expression<Func<Booking, bool>> predicate)
        {
            var result = await _context.Bookings
                .Include(b => b.ClinicClinician.Clinic)
                .Include(b => b.ClinicClinician.Clinician)
                .Include(b => b.Documents)
                .Where(predicate)
                .ToListAsync();

            return _mapper.Mapper.Map<List<BookingDto>>(result);
        }

        public async Task<Booking> GetWithDocumentsAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Documents)
                .SingleOrDefaultAsync(b => b.Id == id);
        }
    }
}