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
        Task<PagingResultDto<BookingDto>> GetForClinicianAsync(PagingDto pagingDto, int patientId);
        Task<PagingResultDto<BookingDto>> GetForPatientAsync(PagingDto pagingDto, int clinicianId);
        Task<Booking> GetWithDocumentsAsync(int id);
    }
}
