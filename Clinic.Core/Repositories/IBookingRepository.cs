using System.Threading.Tasks;

using Clinic.Core.Entities;
using Clinic.Core.DtoModels;
using Clinic.Core.Enums;


namespace Clinic.Core.Repositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<PagingResultDto<BookingDto>> GetForClinicianAsync(PagingDto pagingDto, int patientId, Stage? stage);
        Task<PagingResultDto<BookingDto>> GetForPatientAsync(PagingDto pagingDto, int clinicianId, Stage? stage);
        Task<Booking> GetWithDocumentsAsync(int id);
        void UpdateWithRecalculatingRateAsync(Booking entity);
        float GetClinicianRateAsync(int clinicianId);
    }
}
