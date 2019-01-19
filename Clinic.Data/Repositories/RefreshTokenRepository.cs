using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Automapper.Infrastructure;
using Clinic.Data.Context;

namespace Clinic.Data.Repositories
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
