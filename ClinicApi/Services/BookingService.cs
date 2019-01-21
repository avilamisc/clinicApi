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

        public async Task<ApiResponse> GetAllBookingsForPatientAsync(IEnumerable<Claim> claims, PaginationModel model)
        {
            if (!CheckUserIdInClaims(claims, out int userId)) return new ApiResponse(HttpStatusCode.BadRequest);

            var pagingDto = _mapper.Mapper.Map<PagingDto>(model);
            var bookings = await _unitOfWork.BookingRepository.GetForPatientAsync(pagingDto, userId);
            var result = new PagingResult<IEnumerable<PatientBookingModel>>
            {
                Result = _mapper.Mapper.Map<List<PatientBookingModel>>(bookings),
                TotalAmount = _unitOfWork.BookingRepository.CountForPatien(userId)
            };

            return ApiResponse.Ok(result);
        }

        public async Task<ApiResponse> GetAllBookingsForClinicianAsync(IEnumerable<Claim> claims, PaginationModel model)
        {
            if (!CheckUserIdInClaims(claims, out int userId)) return new ApiResponse(HttpStatusCode.BadRequest);

            var pagingDto = _mapper.Mapper.Map<PagingDto>(model);

            var bookings = await _unitOfWork.BookingRepository.GetForClinicianAsync(pagingDto, userId);
            var result = new PagingResult<IEnumerable<ClinicianBookingModel>>
            {
                Result = _mapper.Mapper.Map<List<ClinicianBookingModel>>(bookings),
                TotalAmount = _unitOfWork.BookingRepository.CountForClinician(userId)
            };

            return ApiResponse.Ok(result);
        }

        public async Task<ApiResponse> CreateBookingAsync(IEnumerable<Claim> claims, HttpRequest request)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
                return ApiResponse.BadRequest();

            PatientBookingModel bookingModel = _mapper.SafeMap<PatientBookingModel>(request.Form);
            if (bookingModel == null)
            {
                return ApiResponse.BadRequest(BookingErrorMessages.WrongBookingDataFormat);
            }

            var clinicClinician = await _unitOfWork.ClinicClinicianRepository.GetClinicClinicianAsync(bookingModel.ClinicId, bookingModel.ClinicianId);

            var validatioErrorResult = CheckPatientBookingModel(bookingModel, clinicClinician, claims);
            if (validatioErrorResult != null) return validatioErrorResult;

            var newBooking = _mapper.Mapper.Map<Booking>(bookingModel);
            newBooking.PatientId = userId;
            newBooking.ClinicClinicianId = clinicClinician.Id;
            newBooking.Documents = CreateNewDocuments(request, userId).ToList();

            var result = _unitOfWork.BookingRepository.Create(newBooking);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                return ApiResponse.InternalError(BookingErrorMessages.UpdateError);
            }

            return ApiResponse.Ok(_mapper.Mapper.Map<PatientBookingModel>(result));
        }

        public async Task<ApiResponse> UpdateBookingAsync(IEnumerable<Claim> claims, HttpRequest request)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
                return new ApiResponse(HttpStatusCode.BadRequest);

            var bookingModel = _mapper.SafeMap<UpdateBookingModel>(request.Form);
            if (bookingModel == null)
            {
                return new ApiResponse(HttpStatusCode.BadRequest, BookingErrorMessages.WrongBookingDataFormat);
            }

            var booking = await _unitOfWork.BookingRepository.GetWithDocumentsAsync(bookingModel.Id);
            if (booking == null) return ApiResponse.NotFound();

            var clinicClinician = await _unitOfWork.ClinicClinicianRepository.GetClinicClinicianAsync(bookingModel.ClinicId, bookingModel.ClinicianId);

            var validatioErrorResult = CheckPatientBookingModel(bookingModel, clinicClinician, claims);
            if (validatioErrorResult != null) return validatioErrorResult;

            var existings = booking.Documents;
            var newDocumets = CreateNewDocuments(request, userId).ToList();
            var documentsToDelete = UpdateBookingDocuments(bookingModel, newDocumets, existings);

            _mapper.Mapper.Map<BookingModel, Booking>(bookingModel, booking);
            booking.ClinicClinicianId = clinicClinician.Id;
            booking.PatientId = booking.PatientId;
            booking.Documents = newDocumets;

            try
            {
                _unitOfWork.BookingRepository.Update(booking);
                _unitOfWork.DocumentRepository.RemoveRange(documentsToDelete);
                await _unitOfWork.SaveChangesAsync();

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
            BookingModel model,
            ClinicClinician clinicClinician,
            IEnumerable<Claim> claims)
        {
            if (!model.IsValid()) return ApiResponse.ValidationError(BookingErrorMessages.ValidationDataError);

            if (clinicClinician == null) return ApiResponse.ValidationError(BookingErrorMessages.MissedClinicClinician);

            return null;
        }

        private IEnumerable<Document> CreateNewDocuments(HttpRequest request, int userId)
        {
            var newDocuments = new List<Document>();
            var files = request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                var filePath = _fileService.UploadFile(files[i]);
                if (filePath == null) continue;

                newDocuments.Add(new Document
                {
                    UserId = userId,
                    Name = files[i].FileName,
                    FilePath = filePath
                });
            }

            return newDocuments;
        }

        private IEnumerable<Document> UpdateBookingDocuments(
            BookingModel model,
            List<Document> documents,
            IEnumerable<Document> existing)
        {
            var documentsToDelete = new List<Document>();

            foreach (var doc in existing)
            {
                if (model.Documents.FirstOrDefault(d => doc.Id == d.Id) != null)
                {
                    documents.Add(doc);
                }
                else
                {
                    _fileService.DeleteFile(doc.FilePath);
                    documentsToDelete.Add(doc);
                }
            }

            return documentsToDelete;
        }
    }
}