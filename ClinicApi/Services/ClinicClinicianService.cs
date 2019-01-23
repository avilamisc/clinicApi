using Clinic.Core.DtoModels;
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

        public async Task<ApiResponse> GetSortdetByDistanceClinicsWithClinician(double longitude = 10, double latitude = 10)
        {
            var geography = GeographyExtensions.CreatePoint(longitude, latitude);

            var sortedClinics = await _unitOfWork.ClinicClinicianRepository.GetSortedByDistanceClinicClinician(geography);

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