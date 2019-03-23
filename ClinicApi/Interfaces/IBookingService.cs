using ClinicApi.Models;
using ClinicApi.Models.Booking;
using ClinicApi.Models.Pagination;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ClinicApi.Interfaces
{
    public interface IBookingService : IServiceBase
    {
        Task<ApiResponse<PagingResult<PatientBookingModel>>> GetAllBookingsForPatientAsync(
            IEnumerable<Claim> claims,
            PaginationModel model);
        Task<ApiResponse<PagingResult<ClinicianBookingModel>>> GetAllBookingsForClinicianAsync(
            IEnumerable<Claim> claims,
            PaginationModel model);
        Task<ApiResponse<BookingResultModel>> CreateBookingAsync(
            IEnumerable<Claim> claims,
            HttpRequest request);
        Task<ApiResponse<BookingResultModel>> UpdateBookingAsync(
            IEnumerable<Claim> claims,
            HttpRequest request);
        Task<ApiResponse<float>> UpdateBookingRateAsync(int id, float rateValue);
        Task<ApiResponse<RemoveResult>> RemoveBookig(int id, IEnumerable<Claim> claims);
    }
}