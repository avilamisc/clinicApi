using System.Collections.Generic;
using System.Threading.Tasks;
using Clinic.Core.UnitOfWork;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Clinic;

namespace ClinicApi.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IApiMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ClinicService(
            IApiMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse> GetAllClinic()
        {
            var clinicDtos = await _unitOfWork.ClinicRepository.GetAllClinicsAsync();

            return ApiResponse.Ok(_mapper.Mapper.Map<IEnumerable<ClinicModel>>(clinicDtos));
        }
    }
}