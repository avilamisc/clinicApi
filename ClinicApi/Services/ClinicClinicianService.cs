using Clinic.Core.DtoModels;
using Clinic.Core.Enums;
using Clinic.Core.GeographyExtensions;
using Clinic.Core.UnitOfWork;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Clinic;
using ClinicApi.Models.ClinicClinician;
using ClinicApi.Models.Clinician;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicApi.Services
{
    public class ClinicClinicianService : IClinicClinicianService
    {
        private readonly IApiMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ClinicClinicianService(
            IApiMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse> GetClinicsWithCliniciansSortdetByDistanceAsync(double longitude, double latitude, ApiVersion version)
        {
            var geography = GeographyExtensions.CreatePoint(longitude, latitude);
            IEnumerable<ClinicLocationDto> sortedClinics = null;

            switch (version)
            {
                case ApiVersion.V1:
                    sortedClinics = await _unitOfWork.ClinicClinicianRepository.GetClinicsWithClinicianSortedByDistanceAsync_V1(geography);
                    break;
                case ApiVersion.V2:
                    sortedClinics = await _unitOfWork.ClinicClinicianRepository.GetClinicsWithClinicianSortedByDistanceAsync_V2(geography);
                    break;
                case ApiVersion.V3:
                    sortedClinics = await _unitOfWork.ClinicClinicianRepository.GetClinicsWithClinicianSortedByDistanceAsync_V3(geography);
                    break;
                default:
                    return ApiResponse.BadRequest();
            }

            var result = new List<ClinicClinicianBase>();
            foreach (var clinic in sortedClinics)
            {
                result.Add(_mapper.Mapper.Map<ClinicWithDistanceModel>(clinic));
                foreach (var clinician in clinic.Clinicians)
                {
                    result.Add(_mapper.Mapper.Map<ClinicianWithDistanceModel>(clinician));
                }
            }

            return ApiResponse.Ok(result);
        }
    }
}