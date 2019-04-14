using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Core.UnitOfWork;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Infrastructure.Constants.ValidationErrorMessages;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Booking;
using ClinicApi.Models.Notification;
using ClinicApi.Models.Pagination;


namespace ClinicApi.Services
{
    public class BookingService : ServiceBase, IBookingService
    {
        private readonly IApiMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ITokenService _tokenService;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;

        private const float MaxRate = 5;

        public BookingService(
            IApiMapper mapper,
            IFileService fileService,
            ITokenService tokenService,
            INotificationService notificationService,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _fileService = fileService;
            _tokenService = tokenService;
            _notificationService = notificationService;
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

        public async Task<ApiResponse<BookingResultModel>> CreateBookingAsync(
            IEnumerable<Claim> claims,
            HttpRequest request)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
                return ApiResponse<BookingResultModel>.BadRequest();

            PatientBookingModel bookingModel = _mapper.SafeMap<PatientBookingModel>(request.Form);
            if (bookingModel == null)
            {
                return ApiResponse<BookingResultModel>
                    .BadRequest(BookingErrorMessages.WrongBookingDataFormat);
            }

            var clinicClinician = await _unitOfWork.ClinicClinicianRepository
                .GetClinicClinicianAsync(bookingModel.ClinicId, bookingModel.ClinicianId);

            var validatioErrorResult = CheckPatientBookingModel(bookingModel, clinicClinician, claims);
            if (validatioErrorResult != null) return validatioErrorResult;

            var newBooking = _mapper.Mapper.Map<Booking>(bookingModel);
            newBooking.PatientId = userId;
            newBooking.ClinicClinicianId = clinicClinician.Id;
            newBooking.CreationDate = DateTime.Now;
            newBooking.UpdateDate = DateTime.Now;
            newBooking.Stage = Stage.Send;
            AddNewDocuments(request, userId, newBooking);

            var result = _unitOfWork.BookingRepository.Create(newBooking);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<BookingResultModel>
                    .Ok(_mapper.Mapper.Map<BookingResultModel>(result));
            }
            catch
            {
                return ApiResponse<BookingResultModel>.InternalError(BookingErrorMessages.UpdateError);
            }
        }

        public async Task<ApiResponse<BookingResultModel>> UpdateBookingAsync(
            IEnumerable<Claim> claims,
            HttpRequest request)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return new ApiResponse<BookingResultModel>(HttpStatusCode.BadRequest);
            }

            var bookingModel = _mapper.SafeMap<UpdateBookingModel>(request.Form);
            if (bookingModel == null)
            {
                return new ApiResponse<BookingResultModel>(
                    HttpStatusCode.BadRequest,
                    BookingErrorMessages.WrongBookingDataFormat);
            }

            var booking = await _unitOfWork.BookingRepository.GetWithDocumentsAsync(bookingModel.Id);
            if (booking == null) return ApiResponse<BookingResultModel>.NotFound();

            var clinicClinician = await _unitOfWork.ClinicClinicianRepository
                .GetClinicClinicianAsync(bookingModel.ClinicId, bookingModel.ClinicianId);

            var validatioErrorResult = CheckPatientBookingModel(bookingModel, clinicClinician, claims);
            if (validatioErrorResult != null) return validatioErrorResult;

            var bookingsToDelete = booking.Documents
                .Where(d => bookingModel.DeletedDocuments.FirstOrDefault(b => b.Id == d.Id) != null)
                .ToList();
            _mapper.Mapper.Map<UpdateBookingModel, Booking>(bookingModel, booking);
            booking.ClinicClinicianId = clinicClinician.Id;
            booking.PatientId = booking.PatientId;
            booking.UpdateDate = DateTime.Now;
            AddNewDocuments(request, userId, booking);
            booking.Documents = booking.Documents
                .Where(b => bookingModel.DeletedDocuments.FirstOrDefault(d => d.Id == b.Id) == null)
                .ToList();

            try
            {
                _unitOfWork.BookingRepository.Update(booking);
                _unitOfWork.DocumentRepository.RemoveRange(bookingsToDelete);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.PatientRepository.GetAsync(booking.PatientId);

                foreach (var file in bookingModel.DeletedDocuments)
                {
                    _fileService.DeleteFile(file.FilePath);
                }

                if (CheckBookingForInProgressTage(booking))
                {
                    booking.Stage = Stage.InProgress;
                    _unitOfWork.BookingRepository.Update(booking);
                    await _unitOfWork.SaveChangesAsync();
                }

                return ApiResponse<BookingResultModel>.Ok(
                    _mapper.Mapper.Map<BookingResultModel>(booking));
            }
            catch
            {
                return new ApiResponse<BookingResultModel>(
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

            if (booking.Stage != Stage.Completed)
            {
                return ApiResponse<float>.ValidationError(BookingErrorMessages.CannotRateNotCompletedBooking);
            }

            try
            {
                booking.Rate = rateValue;
                booking.UpdateDate = DateTime.Now;

                 _unitOfWork.BookingRepository.UpdateWithRecalculatingRateAsync(booking);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                return new ApiResponse<float>(HttpStatusCode.InternalServerError, BookingErrorMessages.UpdateError);
            }
            return ApiResponse<float>.Ok(booking.Rate.Value);
        }

        public async Task<ApiResponse<Stage>> UpdateStageAsync(IEnumerable<Claim> claims, int id, Stage newStage)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return new ApiResponse<Stage>(HttpStatusCode.BadRequest);
            }

            var booking = await _unitOfWork.BookingRepository.GetAsync(id, b => b.ClinicClinician);
            if (booking == null)
            {
                return ApiResponse<Stage>.BadRequest(BookingErrorMessages.UnexistingBooking);
            }

            if (booking.PatientId != userId && booking.ClinicClinician.ClinicianId != userId)
            {
                return ApiResponse<Stage>.BadRequest(BookingErrorMessages.AccessIsDenied);
            }

            switch (newStage)
            {
                case Stage.Confirmed:
                    if (booking.Stage != Stage.Send)
                    {
                        return ApiResponse<Stage>.ValidationError(BookingErrorMessages.CannotUpdateNotSendBooking);
                    }
                    break;
                case Stage.InProgress:
                    if (booking.Stage != Stage.Send && booking.Stage != Stage.Confirmed)
                    {
                        return ApiResponse<Stage>.ValidationError(BookingErrorMessages.CannotUpdateNotSendOrConfirmedBooking);
                    }
                    break;
                case Stage.Rejected:
                    if (booking.Stage != Stage.Send)
                    {
                        return ApiResponse<Stage>.ValidationError(BookingErrorMessages.CannotUpdateNotSendBooking);
                    }
                    break;
                case Stage.Canceled:
                    if (booking.Stage != Stage.Confirmed && booking.Stage != Stage.InProgress)
                    {
                        return ApiResponse<Stage>.ValidationError(BookingErrorMessages.CannotUpdateNotConfirmedOrInProgressBooking);
                    }
                    break;
                case Stage.Completed:
                    if (booking.Stage != Stage.InProgress)
                    {
                        return ApiResponse<Stage>.ValidationError(BookingErrorMessages.CannotUpdateNotInProgressBooking);
                    }
                    break;
                default:
                    return ApiResponse<Stage>.BadRequest();
            }

            try
            {
                booking.Stage = newStage;
                _unitOfWork.BookingRepository.Update(booking);
                await _unitOfWork.SaveChangesAsync();

                var notification = new CreateNotificationModel
                {
                    Content = $"Booking {booking.Name} has been updated to {newStage} stage.",
                    CreationDate = DateTime.Now,
                    UserId = booking.PatientId
                };
                await _notificationService.CreateNotificationAsync(claims, notification);
            }
            catch (InvalidOperationException)
            {
                return ApiResponse<Stage>.InternalError(BookingErrorMessages.UpdateError);
            }

            return ApiResponse<Stage>.Ok(booking.Stage);
        }

        private ApiResponse<BookingResultModel> CheckPatientBookingModel(
            BookingModel model,
            ClinicClinician clinicClinician,
            IEnumerable<Claim> claims)
        {
            var validationError = model.CheckValidationError();
            if (validationError != null)
            {
                return ApiResponse<BookingResultModel>
                    .ValidationError($"{BookingErrorMessages.ValidationDataError}\n {validationError}");
            }

            if (clinicClinician == null)
            {
                return ApiResponse<BookingResultModel>
                    .ValidationError(BookingErrorMessages.MissedClinicClinician);
            }

            return null;
        }

        public async Task<ApiResponse<RemoveResult<BookingResultModel>>> RemoveBookig(int id, IEnumerable<Claim> claims)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return ApiResponse<RemoveResult<BookingResultModel>>.BadRequest();
            }

            var booking = await _unitOfWork.BookingRepository.GetFirstAsync(
                b => b.Id == id,
                b => b.ClinicClinician,
                b => b.Documents);
            if (booking == null)
            {
                return ApiResponse<RemoveResult<BookingResultModel>>.Ok(
                    RemoveResult<BookingResultModel>.Failed(BookingErrorMessages.UnexistingBooking));
            }

            if (booking.PatientId != userId && booking.ClinicClinician.ClinicianId != userId)
            {
                return ApiResponse<RemoveResult<BookingResultModel>>.Ok(
                    RemoveResult<BookingResultModel>.Failed(BookingErrorMessages.PermissionsToDelete));
            }

            try
            {
                var resultValue = _mapper.Mapper.Map<BookingResultModel>(booking);
                RemoveUsersDocuments(booking);
                _unitOfWork.DocumentRepository.RemoveRange(booking.Documents);
                _unitOfWork.BookingRepository.Remove(booking);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<RemoveResult<BookingResultModel>>.Ok(
                    RemoveResult<BookingResultModel>.Removed(BookingErrorMessages.SuccessfulDelete, resultValue));
            }
            catch
            {
                return ApiResponse<RemoveResult<BookingResultModel>>.InternalError();
            }
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

        private void RemoveUsersDocuments(Booking booking)
        {
            foreach (var doc in booking.Documents)
            {
                _fileService.DeleteFile(doc.FilePath);
            }

            booking.Documents = new List<Document>();
        }

        private bool CheckBookingForInProgressTage(Booking booking)
        {
            return booking.HeartRate.HasValue &&
                booking.Height.HasValue &&
                booking.Weight.HasValue &&
                (booking.Stage == Stage.Send || booking.Stage == Stage.Confirmed);
        }
    }
}