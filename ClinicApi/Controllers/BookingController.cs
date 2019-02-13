﻿using ClinicApi.Infrastructure.Auth;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Pagination;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ClinicApi.Controllers
{
    [RoutePrefix("api/bookings")]
    public class BookingController : ApiController
    {
        private IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [BearerAuthorization(Roles = "Patient")]
        [Route("patient")]
        public async Task<IHttpActionResult> PatientBookings([FromUri]PaginationModel model)
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _bookingService.GetAllBookingsForPatientAsync(identity.Claims, model));
        }

        [HttpGet]
        [BearerAuthorization(Roles = "Clinician")]
        [Route("clinician")]
        public async Task<IHttpActionResult> ClinicianBookings([FromUri]PaginationModel model)
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _bookingService.GetAllBookingsForClinicianAsync(identity.Claims, model));
        }

        [HttpPost]
        [BearerAuthorization(Roles = "Patient")]
        [Route("")]
        public async Task<IHttpActionResult> CreateBooking()
        {
            if (!Request.Content.IsMimeMultipartContent()) return Ok(ApiResponse.UnsupportedMediaType());

            var identity = (ClaimsIdentity)User.Identity;
            var result = await _bookingService.CreateBookingAsync(identity.Claims, HttpContext.Current.Request);

            return Ok(result);
        }

        [HttpPut]
        [BearerAuthorization]
        [Route("")]
        public async Task<IHttpActionResult> UpdateBooking()
        {
            if (!Request.Content.IsMimeMultipartContent()) return Ok(ApiResponse.UnsupportedMediaType());

            var identity = (ClaimsIdentity)User.Identity;
            var result = await _bookingService.UpdateBookingAsync(identity.Claims, HttpContext.Current.Request);

            return Ok(result);
        }
    }
}