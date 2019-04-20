using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Context;

namespace Clinic.Data.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ClinicDb context) : base(context)
        {
        }
    }
}
