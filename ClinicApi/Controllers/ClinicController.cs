using Clinic.Core.Enums;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClinicApi.Controllers
{
    [RoutePrefix("api/clinics")]
    [Authorize]
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
        public async Task<IHttpActionResult> Clinics(double longitude = 0, double latitude = 0)
        {
            return Ok(await _clinicService.GetAllClinicAsync(longitude, latitude));
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> ClinicById(int id)
        {
            return Ok(await _clinicService.GetClinicByIdAsync(id));
        }

        [HttpGet]
        [Route("clinicians")]
        public async Task<IHttpActionResult> ClinicClinicians(double longitude = 0, double latitude = 0, ApiVersion v = ApiVersion.V3)
        {
            return Ok(await _clinicClinicianServiceV1.GetClinicsWithCliniciansSortdetByDistanceAsync(longitude, latitude, v));
        }
    }
}
