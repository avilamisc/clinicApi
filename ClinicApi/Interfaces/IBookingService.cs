using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

using Clinic.Core.Entities;
using Clinic.Core.Enums;
using ClinicApi.Models;
using ClinicApi.Models.Booking;
using ClinicApi.Models.Pagination;


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
        Task<ApiResponse<RemoveResult<BookingResultModel>>> RemoveBookig(int id, IEnumerable<Claim> claims);
        Task<ApiResponse<Stage>> UpdateStageAsync(IEnumerable<Claim> claims, int id, Stage newStage);
    }
}