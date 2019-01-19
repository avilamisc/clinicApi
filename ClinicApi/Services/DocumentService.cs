using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Clinic.Core.UnitOfWork;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Infrastructure.Constants;
using ClinicApi.Interfaces;
using ClinicApi.Models.Document;

namespace ClinicApi.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IApiMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DocumentService(
            IApiMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<DocumentModel> GetDocumentByIdAsync(IEnumerable<Claim> claims, int id)
        {
            var document = await _unitOfWork.DocumentRepository.GetAsync(id);

            if (!Int32.TryParse(claims.Single(c => c.Type == ApiConstants.UserIdClaimName).Value, out int userId))
            {
                return null;
            }

            return document.UserId == userId
                ? _mapper.Mapper.Map<DocumentModel>(document)
                : null;

        }
    }
}