using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Booking;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace ClinicApi.Controllers
{
    public class BookingController : ApiController
    {
        private IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Authorize(Roles = "Patient")]
        [Route("api/booking/patient")]
        public async Task<IHttpActionResult> PatientBookings()
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _bookingService.GetAllBookingsForPatientAsync(identity.Claims));
        }

        [HttpGet]
        [Authorize(Roles = "Clinician")]
        [Route("api/booking/clinician")]
        public async Task<IHttpActionResult> ClinicianBookings()
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _bookingService.GetAllBookingsForClinicianAsync(identity.Claims));
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        [Route("api/booking")]
        public async Task<IHttpActionResult> CreateBooking()
        {
            if (!Request.Content.IsMimeMultipartContent()) return Ok(new ApiResponse(HttpStatusCode.UnsupportedMediaType));

            var identity = (ClaimsIdentity)User.Identity;
            var result = await _bookingService.CreateBookingAsync(identity.Claims, HttpContext.Current.Request);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Patient")]
        [Route("api/booking")]
        public async Task<IHttpActionResult> UpdateBooking()
        {
            if (!Request.Content.IsMimeMultipartContent()) return Ok(new ApiResponse(HttpStatusCode.UnsupportedMediaType));

            var identity = (ClaimsIdentity)User.Identity;
            var result = await _bookingService.UpdateBookingAsync(identity.Claims, HttpContext.Current.Request);

            return Ok(result);
        }
    }
}