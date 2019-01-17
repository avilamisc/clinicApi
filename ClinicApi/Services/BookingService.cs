using Clinic.Core.Entities;
using Clinic.Core.UnitOfWork;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Infrastructure.Constants;
using ClinicApi.Infrastructure.Constants.ValidationErrorMessages;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Booking;
using ClinicApi.Models.Document;
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

        public async Task<ApiResponse> GetAllBookingsForPatientAsync(IEnumerable<Claim> claims)
        {
            if (!CheckUserIdInClaims(claims, out int userId)) return new ApiResponse(HttpStatusCode.BadRequest);

            var bookings = await _unitOfWork.BookingRepository.GetForPatientAsync(b => b.PatientId == userId);

            return ApiResponse.Ok(_mapper.Mapper.Map<List<PatientBookingModel>>(bookings));
        }

        public async Task<ApiResponse> GetAllBookingsForClinicianAsync(IEnumerable<Claim> claims)
        {
            if (!CheckUserIdInClaims(claims, out int userId)) return new ApiResponse(HttpStatusCode.BadRequest);

            var bookings = await _unitOfWork.BookingRepository.GetForClinicianAsync(b => b.ClinicClinician.ClinicianId == userId);

            return ApiResponse.Ok(_mapper.Mapper.Map<List<ClinicianBookingModel>>(bookings));
        }

        public async Task<ApiResponse> CreateBookingAsync(IEnumerable<Claim> claims, HttpRequest request)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
                return new ApiResponse(HttpStatusCode.BadRequest);

            PatientBookingModel bookingModel = _mapper.SafeMap<PatientBookingModel>(request.Form);
            if (bookingModel == null)
            {
                return new ApiResponse(HttpStatusCode.BadRequest, BookingErrorMessages.WrongDocumentsDataFormat);
            }

            if (!CheckAndPopulateUserIdAndFilesFormRequest(request, userId, bookingModel))
            {
                return new ApiResponse(HttpStatusCode.NotFound, BookingErrorMessages.MissedFile);
            }

            var clinicClinician = await GetClinicClinicianAsync(bookingModel.ClinicId, bookingModel.ClinicianId);

            var validatioErrorResult = CheckPatientBookingModel(userId, bookingModel, clinicClinician, claims);
            if (validatioErrorResult != null) return validatioErrorResult;

            var newBooking = _mapper.Mapper.Map<Booking>(bookingModel);
            newBooking.PatientId = userId;
            newBooking.ClinicClinicianId = clinicClinician.Id;

            var result = _unitOfWork.BookingRepository.Create(newBooking);
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.ClinicClinicianRepository.UploadClinicAsync(newBooking.ClinicClinician);
            await _unitOfWork.ClinicClinicianRepository.UploadClinicianAsync(newBooking.ClinicClinician);

            return ApiResponse.Ok(_mapper.Mapper.Map<PatientBookingModel>(result));
        }

        public async Task<ApiResponse> UpdateBookingAsync(IEnumerable<Claim> claims, HttpRequest request)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
                return new ApiResponse(HttpStatusCode.BadRequest);

            var bookingModel = _mapper.SafeMap<UpdateBookingModel>(request.Form);
            if (bookingModel == null)
            {
                return new ApiResponse(HttpStatusCode.BadRequest, BookingErrorMessages.WrongDocumentsDataFormat);
            }

            if (!CheckAndPopulateUserIdAndFilesFormRequest(request, userId, bookingModel))
            {
                return new ApiResponse(HttpStatusCode.NotFound, BookingErrorMessages.MissedFile);
            }

            var booking = await _unitOfWork.BookingRepository.GetWithDocumentsAsync(bookingModel.Id);
            if (booking == null || booking.PatientId != userId) return new ApiResponse(HttpStatusCode.NotFound);

            var clinicClinician = await GetClinicClinicianAsync(bookingModel.ClinicId, bookingModel.ClinicianId);

            var validatioErrorResult = CheckPatientBookingModel(userId, bookingModel, clinicClinician, claims);
            if (validatioErrorResult != null) return validatioErrorResult;

            var existingIds = booking.Documents;

            _mapper.Mapper.Map<BookingModel, Booking>(bookingModel, booking);
            booking.ClinicClinicianId = clinicClinician.Id;
            booking.PatientId = booking.PatientId;
            booking.Documents = UpdateAndGetNewDocuments(bookingModel, existingIds).ToList();

            try
            {
                _unitOfWork.BookingRepository.Update(booking);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.ClinicClinicianRepository.UploadClinicAsync(booking.ClinicClinician);
                await _unitOfWork.ClinicClinicianRepository.UploadClinicianAsync(booking.ClinicClinician);

                return ApiResponse.Ok(_mapper.Mapper.Map<PatientBookingModel>(booking));
            }
            catch
            {
                return new ApiResponse(HttpStatusCode.InternalServerError, BookingErrorMessages.UpdateError);
            }
        }

        private bool CheckUserIdInClaims(IEnumerable<Claim> claims, out int userId)
        {
            return Int32.TryParse(claims.Single(c => c.Type == ApiConstants.UserIdClaimName).Value, out userId);
        }

        private ApiResponse CheckPatientBookingModel(
            int userId,
            BookingModel model,
            ClinicClinician clinicClinician,
            IEnumerable<Claim> claims)
        {
            if (!model.IsValid()) return ApiResponse.ValidationError(BookingErrorMessages.ValidationDataError);

            if (clinicClinician == null) return ApiResponse.ValidationError(BookingErrorMessages.MissedClinicClinician);

            return null;
        }

        private async Task<ClinicClinician> GetClinicClinicianAsync(int clinicId, int clinicianId)
        {
            return await _unitOfWork.ClinicClinicianRepository.GetSingleAsync(c => c.ClinicId == clinicId
                && c.ClinicianId == clinicianId);
        }

        private bool CheckAndPopulateUserIdAndFilesFormRequest(HttpRequest request, int userId, BookingModel model)
        {            
            var files = request.Files;

            foreach (var document in model.Documents)
            {
                var file = files.Get(document.FileKey);
                if (file == null) return false;

                var filePath = _fileService.UploadFile(file);
                if (filePath == null) return false;

                document.FilePath = filePath;
                document.UserId = userId;
            }

            return true;
        }

        private IEnumerable<Document> UpdateAndGetNewDocuments(UpdateBookingModel model, IEnumerable<Document> existing)
        {
            var newDocuments = new List<Document>();

            foreach (var doc in model.Documents)
            {
                var existingEntity = existing.FirstOrDefault(d => d.Id == doc.Id);

                if (existingEntity != null)
                {
                    _fileService.DeleteFile(existingEntity.FilePath);
                    _mapper.Mapper.Map<DocumentModel, Document>(doc, existingEntity);
                    _unitOfWork.DocumentRepository.Update(existingEntity);
                    newDocuments.Add(existingEntity);
                }
                else
                {
                    newDocuments.Add(_mapper.Mapper.Map<Document>(doc));
                }
            }

            return newDocuments;
        }
    }
}