using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Infrastructure.Auth;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Clinician;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClinicApi.Controllers
{
    [RoutePrefix("api/clinicians")]
    [BearerAuthorization]
    public class ClinicianController : ApiController
    {
        private readonly IApiMapper _mapper;
        private readonly IClinicianService _clinicianService;

        public ClinicianController(
            IClinicianService clinicianService,
            IApiMapper mapper)
        {
            _clinicianService = clinicianService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ClinicianModel>>))]
        public async Task<IHttpActionResult> Clinicians(int? clinicId = null)
        {
            return Ok(await _clinicianService.GetCliniciansForClinic(clinicId));
        }
    }
}
