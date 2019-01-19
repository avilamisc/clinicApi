using ClinicApi.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClinicApi.Controllers
{
    public class ClinicController : ApiController
    {
        private readonly IClinicService _clinicService;

        public ClinicController(IClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/clinics")]
        public async Task<IHttpActionResult> Clinics()
        {
            return Ok(await _clinicService.GetAllClinic());
        }
    }
}
