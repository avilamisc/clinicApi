using ClinicApi.Models;
using ClinicApi.Models.Booking;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ClinicApi.Interfaces
{
    public interface IBookingService
    {
        Task<ApiResponse> GetAllBookingsForPatientAsync(IEnumerable<Claim> claims);
        Task<ApiResponse> GetAllBookingsForClinicianAsync(IEnumerable<Claim> claims);
        Task<ApiResponse> CreateBookingAsync(IEnumerable<Claim> claims, HttpRequest request);
        Task<ApiResponse> UpdateBookingAsync(IEnumerable<Claim> claims, HttpRequest request);
    }
}