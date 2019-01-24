using Clinic.Core.GeographyExtensions;
using Clinic.Core.UnitOfWork;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Clinic;
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

        public async Task<ApiResponse> GetSortdetByDistanceClinicsWithClinician(double longitude, double latitude)
        {
            var geography = GeographyExtensions.CreatePoint(longitude, latitude);

            var sortedClinics = await _unitOfWork.ClinicClinicianRepository.GetClinicClinicianSortedByDistance(geography);

            var result = new List<object>();
            foreach (var clinic in sortedClinics)
            {
                result.Add(_mapper.Mapper.Map<ClinicWithDistanceModel>(clinic));
                foreach (var clinician in clinic.Clinicians)
                {
                    result.Add(_mapper.Mapper.Map<ClinicianModel>(clinician));
                }
            }

            return ApiResponse.Ok(result);
        }

        public async Task<ApiResponse> GetSortdetByDistanceClinicsWithClinicianV2(double longitude, double latitude)
        {
            var geography = GeographyExtensions.CreatePoint(longitude, latitude);

            var sortedClinics = await _unitOfWork.ClinicClinicianRepository.GetClinicClinicianSortedByDistanceV2(geography);

            var result = new List<object>();
            foreach (var clinic in sortedClinics)
            {
                result.Add(_mapper.Mapper.Map<ClinicWithDistanceModel>(clinic));
                foreach (var clinician in clinic.Clinicians)
                {
                    result.Add(_mapper.Mapper.Map<ClinicianModel>(clinician));
                }
            }

            return ApiResponse.Ok(result);
        }

        public async Task<ApiResponse> GetSortdetByDistanceClinicsWithClinicianV3(double longitude, double latitude)
        {
            var geography = GeographyExtensions.CreatePoint(longitude, latitude);

            var sortedClinics = await _unitOfWork.ClinicClinicianRepository.GetClinicClinicianSortedByDistanceV3(geography);

            var result = new List<object>();
            foreach (var clinic in sortedClinics)
            {
                result.Add(_mapper.Mapper.Map<ClinicWithDistanceModel>(clinic));
                foreach (var clinician in clinic.Clinicians)
                {
                    result.Add(_mapper.Mapper.Map<ClinicianModel>(clinician));
                }
            }

            return ApiResponse.Ok(result);
        }
    }
}