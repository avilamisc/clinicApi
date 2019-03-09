using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using Clinic.Core.UnitOfWork;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Infrastructure.Constants;
using ClinicApi.Infrastructure.Constants.ValidationErrorMessages;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Booking;
using ClinicApi.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ClinicApi.Services
{
    public class BookingService : IBookingService
    {
        private readonly IApiMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        private const float MaxRate = 5;

        public BookingService(
            IApiMapper mapper,
            IFileService fileService,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _fileService = fileService;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<PagingResult<PatientBookingModel>>> GetAllBookingsForPatientAsync(
            IEnumerable<Claim> claims,
            PaginationModel model)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return new ApiResponse<PagingResult<PatientBookingModel>>(HttpStatusCode.BadRequest);
            }

            var pagingDto = _mapper.Mapper.Map<PagingDto>(model);
            var pagingResult = await _unitOfWork.BookingRepository.GetForPatientAsync(pagingDto, userId);

            return ApiResponse<PagingResult<PatientBookingModel>>.Ok(
                new PagingResult<PatientBookingModel>
                {
                    DataCollection = _mapper.Mapper
                        .Map<IEnumerable<PatientBookingModel>>(pagingResult.DataColection),
                    TotalCount = pagingResult.TotalCount
                });
        }

        public async Task<ApiResponse<PagingResult<ClinicianBookingModel>>> GetAllBookingsForClinicianAsync(
            IEnumerable<Claim> claims,
            PaginationModel model)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return new ApiResponse<PagingResult<ClinicianBookingModel>>(HttpStatusCode.BadRequest);
            }

            var pagingDto = _mapper.Mapper.Map<PagingDto>(model);

            var pagingResult = await _unitOfWork.BookingRepository
                .GetForClinicianAsync(pagingDto, userId);

            return ApiResponse<PagingResult<ClinicianBookingModel>>.Ok(
                new PagingResult<ClinicianBookingModel>
                {
                    DataCollection = _mapper.Mapper
                        .Map<IEnumerable<ClinicianBookingModel>>(pagingResult.DataColection),
                    TotalCount = pagingResult.TotalCount
                });
        }

        public async Task<ApiResponse<PatientBookingModel>> CreateBookingAsync(
            IEnumerable<Claim> claims,
            HttpRequest request)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
                return ApiResponse<PatientBookingModel>.BadRequest();

            PatientBookingModel bookingModel = _mapper.SafeMap<PatientBookingModel>(request.Form);
            if (bookingModel == null)
            {
                return ApiResponse<PatientBookingModel>
                    .BadRequest(BookingErrorMessages.WrongBookingDataFormat);
            }

            var clinicClinician = await _unitOfWork.ClinicClinicianRepository
                .GetClinicClinicianAsync(bookingModel.ClinicId, bookingModel.ClinicianId);

            var validatioErrorResult = CheckPatientBookingModel(bookingModel, clinicClinician, claims);
            if (validatioErrorResult != null) return validatioErrorResult;

            var newBooking = _mapper.Mapper.Map<Booking>(bookingModel);
            newBooking.PatientId = userId;
            newBooking.ClinicClinicianId = clinicClinician.Id;
            AddNewDocuments(request, userId, newBooking);

            var result = _unitOfWork.BookingRepository.Create(newBooking);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<PatientBookingModel>
                    .Ok(_mapper.Mapper.Map<PatientBookingModel>(result));
            }
            catch
            {
                return ApiResponse<PatientBookingModel>.InternalError(BookingErrorMessages.UpdateError);
            }
        }

        public async Task<ApiResponse<PatientBookingModel>> UpdateBookingAsync(
            IEnumerable<Claim> claims,
            HttpRequest request)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return new ApiResponse<PatientBookingModel>(HttpStatusCode.BadRequest);
            }

            var bookingModel = _mapper.SafeMap<UpdateBookingModel>(request.Form);
            if (bookingModel == null)
            {
                return new ApiResponse<PatientBookingModel>(
                    HttpStatusCode.BadRequest,
                    BookingErrorMessages.WrongBookingDataFormat);
            }

            var booking = await _unitOfWork.BookingRepository.GetWithDocumentsAsync(bookingModel.Id);
            if (booking == null) return ApiResponse<PatientBookingModel>.NotFound();

            var clinicClinician = await _unitOfWork.ClinicClinicianRepository
                .GetClinicClinicianAsync(bookingModel.ClinicId, bookingModel.ClinicianId);

            var validatioErrorResult = CheckPatientBookingModel(bookingModel, clinicClinician, claims);
            if (validatioErrorResult != null) return validatioErrorResult;

            var bookingsToDelete = booking.Documents
                .Where(d => bookingModel.DeletedDocuments.FirstOrDefault(b => b.Id == d.Id) != null)
                .ToList();
            _mapper.Mapper.Map<BookingModel, Booking>(bookingModel, booking);
            booking.ClinicClinicianId = clinicClinician.Id;
            booking.PatientId = booking.PatientId;
            AddNewDocuments(request, userId, booking);
            booking.Documents = booking.Documents
                .Where(b => bookingModel.DeletedDocuments.FirstOrDefault(d => d.Id == b.Id) == null)
                .ToList();

            try
            {
                _unitOfWork.BookingRepository.Update(booking);
                _unitOfWork.DocumentRepository.RemoveRange(bookingsToDelete);
                await _unitOfWork.SaveChangesAsync();

                foreach (var file in bookingModel.DeletedDocuments)
                {
                    _fileService.DeleteFile(file.FilePath);
                }

                return ApiResponse<PatientBookingModel>.Ok(
                    _mapper.Mapper.Map<PatientBookingModel>(booking));
            }
            catch
            {
                return new ApiResponse<PatientBookingModel>(
                    HttpStatusCode.InternalServerError,
                    BookingErrorMessages.UpdateError);
            }
        }

        public async Task<ApiResponse<float>> UpdateBookingRateAsync(int id, float rateValue)
        {
            if (rateValue > MaxRate || rateValue < 0)
            {
                return ApiResponse<float>.BadRequest(BookingErrorMessages.BadRateValue(MaxRate));
            }

            var booking = await _unitOfWork.BookingRepository.GetFirstAsync(b => b.Id == id);
            if (booking == null)
            {
                return ApiResponse<float>.BadRequest(BookingErrorMessages.UnexistingBooking);
            }

            try
            {
                booking.Rate = rateValue;
                 _unitOfWork.BookingRepository.UpdateWithRecalculatingRateAsync(booking);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                return new ApiResponse<float>(HttpStatusCode.InternalServerError, BookingErrorMessages.UpdateError);
            }
            return ApiResponse<float>.Ok(booking.ClinicClinician.Clinician.Rate);
        }

        private bool CheckUserIdInClaims(IEnumerable<Claim> claims, out int userId)
        {
            return Int32.TryParse(
                claims.Single(c => c.Type == ApiConstants.UserIdClaimName).Value,
                out userId);
        }

        private ApiResponse<PatientBookingModel> CheckPatientBookingModel(
            BookingModel model,
            ClinicClinician clinicClinician,
            IEnumerable<Claim> claims)
        {
            if (!model.IsValid())
            {
                return ApiResponse<PatientBookingModel>.ValidationError(BookingErrorMessages.ValidationDataError);
            }

            if (clinicClinician == null)
            {
                return ApiResponse<PatientBookingModel>.ValidationError(BookingErrorMessages.MissedClinicClinician);
            }

            return null;
        }

        private void AddNewDocuments(HttpRequest request, int userId, Booking booking)
        {
            var files = request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                var filePath = _fileService.UploadFile(files[i]);
                if (filePath == null) continue;

                booking.Documents.Add(new Document
                {
                    UserId = userId,
                    Name = files[i].FileName,
                    FilePath = filePath
                });
            }
        }
    }
}