using ClinicApi.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClinicApi.Controllers
{
    [RoutePrefix("api/clinics")]
    [Authorize]
    public class ClinicController : ApiController
    {
        private readonly IClinicService _clinicService;
        private readonly IClinicClinicianService _clinicClinicianService;

        public ClinicController(
            IClinicService clinicService,
            IClinicClinicianService clinicClinicianService)
        {
            _clinicService = clinicService;
            _clinicClinicianService = clinicClinicianService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Clinics()
        {
            return Ok(await _clinicService.GetAllClinic());
        }

        [HttpGet]
        [Route("{longitude}/{latitude}")]
        public async Task<IHttpActionResult> ClinicClinicians(double longitude, double latitude)
        {
            var result = await _clinicClinicianService.GetSortdetByDistanceClinicsWithClinician(longitude, latitude);

            return Ok(result);
        }
    }
}
