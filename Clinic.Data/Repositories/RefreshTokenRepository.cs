using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Automapper.Infrastructure;
using Clinic.Data.Context;
using Clinic.Domain.Repositories.Base;

namespace Clinic.Domain.Repositories.Concrete
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly IDataMapper _mapper;

        public RefreshTokenRepository(
            IDataMapper mapper,
            ClinicDb context) : base(context)
        {
            _mapper = mapper;
        }
    }
}
