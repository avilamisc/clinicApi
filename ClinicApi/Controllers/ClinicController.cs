using Clinic.Core.Enums;
using ClinicApi.Infrastructure.Auth;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Clinic;
using ClinicApi.Models.Pagination;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClinicApi.Controllers
{
    [RoutePrefix("api/clinics")]
    [BearerAuthorization]
    public class ClinicController : ApiController
    {
        private readonly IClinicService _clinicService;
        private readonly IClinicClinicianService _clinicClinicianServiceV1;

        public ClinicController(
            IClinicService clinicService,
            IClinicClinicianService clinicClinicianServiceV1)
        {
            _clinicService = clinicService;
            _clinicClinicianServiceV1 = clinicClinicianServiceV1;
        }

        [HttpGet]
        [Route("")]
        [OverrideAuthorization]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ClinicModel>>))]
        public async Task<IHttpActionResult> Clinics(
            [FromUri]PaginationModel paginationModel, double longitude = 0, double latitude = 0)
        {
            return Ok(await _clinicService.GetAllClinicAsync(paginationModel, longitude, latitude));
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<ClinicModel>))]
        public async Task<IHttpActionResult> ClinicById(int id)
        {
            return Ok(await _clinicService.GetClinicByIdAsync(id));
        }

        [HttpGet]
        [Route("clinicians")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<ClinicModel>))]
        public async Task<IHttpActionResult> ClinicClinicians(
            [FromUri]LocationPagingModel pagingModel, ApiVersion v = ApiVersion.V3)
        {
            return Ok(await _clinicClinicianServiceV1
                .GetClinicsWithCliniciansSortdetByDistanceAsync(pagingModel, v));
        }
    }
}
