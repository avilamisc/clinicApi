using System.Collections.Generic;
using System.Threading.Tasks;
using Clinic.Core.UnitOfWork;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Clinician;

namespace ClinicApi.Services
{
    public class ClinicianService : IClinicianService
    {
        private readonly IApiMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ClinicianService(
            IApiMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<ClinicianModel>>> GetCliniciansForClinic(int? clinicId)
        {
            var clinicianDtos = await _unitOfWork.ClinicianRepository.GetCliniciansAsync(clinicId);

            return ApiResponse<IEnumerable<ClinicianModel>>.Ok(
                _mapper.Mapper.Map<IEnumerable<ClinicianModel>>(clinicianDtos));
        }
    }
}