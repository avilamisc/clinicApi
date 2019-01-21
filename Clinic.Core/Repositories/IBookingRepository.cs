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
        Task<IEnumerable<BookingDto>> GetForClinicianAsync(PagingDto pagingDto, int patientId);
        Task<IEnumerable<BookingDto>> GetForPatientAsync(PagingDto pagingDto, int clinicianId);
        Task<Booking> GetWithDocumentsAsync(int id);
        int CountForPatien(int patinetId);
        int CountForClinician(int clinicianId);
    }
}
