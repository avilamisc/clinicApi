using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClinicApi.Controllers
{
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

        [Authorize]
        [HttpGet]
        [Route("api/clinicians/{clinicId}")]
        public async Task<IHttpActionResult> Clinicians(int clinicId)
        {
            return Ok(await _clinicianService.GetCliniciansForClinic(clinicId));
        }
    }
}
