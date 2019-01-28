using System.Collections.Generic;
using System.Threading.Tasks;
using Clinic.Core.DtoModels;
using Clinic.Core.GeographyExtensions;
using Clinic.Core.UnitOfWork;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Clinic;
using ClinicApi.Models.Pagination;

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

        public async Task<ApiResponse> GetAllClinicAsync(PaginationModel paging, double longitude, double latitude)
        {
            var location = GeographyExtensions.CreatePoint(longitude, latitude);
            var pagingDto = _mapper.Mapper.Map<PagingDto>(paging);

            var clinicDtos = await _unitOfWork.ClinicRepository.GetAllClinicsAsync(pagingDto, location);

            return ApiResponse.Ok(_mapper.Mapper.Map<IEnumerable<ClinicModel>>(clinicDtos));
        }

        public async Task<ApiResponse> GetClinicByIdAsync(int id)
        {
            var result = await _unitOfWork.ClinicRepository.GetAsync(id);

            if (result == null) return ApiResponse.NotFound();

            return ApiResponse.Ok(_mapper.Mapper.Map<ClinicModel>(result));
        }
    }
}