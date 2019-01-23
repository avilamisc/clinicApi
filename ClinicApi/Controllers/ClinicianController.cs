using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClinicApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/clinicians")]
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
        [Route("{clinicId}")]
        public async Task<IHttpActionResult> Clinicians(int clinicId)
        {
            return Ok(await _clinicianService.GetCliniciansForClinic(clinicId));
        }
    }
}
