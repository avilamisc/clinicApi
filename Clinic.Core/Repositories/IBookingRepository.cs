using Clinic.Core.Entities;
using Clinic.Core.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Clinic.Core.Repositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<BookingDto>> GetForPatientAsync(Expression<Func<Booking, bool>> predicate);
        Task<IEnumerable<BookingDto>> GetForClinicianAsync(Expression<Func<Booking, bool>> predicate);
        Task<Booking> GetWithDocumentsAsync(int id);
    }
}
