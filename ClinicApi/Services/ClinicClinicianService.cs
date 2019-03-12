using Clinic.Core.DtoModels;
using Clinic.Core.Enums;
using Clinic.Core.GeographyExtensions;
using Clinic.Core.UnitOfWork;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.ClinicClinician;
using ClinicApi.Models.Pagination;
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

        public async Task<ApiResponse<IEnumerable<ClinicWithDistanceModel>>> GetClinicsWithCliniciansSortdetByDistanceAsync(
            LocationPagingModel pagingModel,
            ApiVersion version)
        {
            var geography = GeographyExtensions.CreatePoint(pagingModel.Longitude, pagingModel.Latitude);
            IEnumerable<ClinicLocationDto> sortedClinics = null;

            switch (version)
            {
                case ApiVersion.V1:
                    sortedClinics = await _unitOfWork.ClinicClinicianRepository
                        .GetClinicsWithClinicianSortedByDistanceAsync_V1(geography, pagingModel.Count);
                    break;
                case ApiVersion.V2:
                    sortedClinics = await _unitOfWork.ClinicClinicianRepository
                        .GetClinicsWithClinicianSortedByDistanceAsync_V2(geography, pagingModel.Count);
                    break;
                case ApiVersion.V3:
                    sortedClinics = await _unitOfWork.ClinicClinicianRepository
                        .GetClinicsWithClinicianSortedByDistanceAsync_V3(geography, pagingModel.Count);
                    break;
                default:
                    return ApiResponse<IEnumerable<ClinicWithDistanceModel>>.BadRequest();
            }

            return ApiResponse<IEnumerable<ClinicWithDistanceModel>>.Ok(
                _mapper.Mapper.Map<IEnumerable< ClinicWithDistanceModel>>(sortedClinics));
        }
    }
}