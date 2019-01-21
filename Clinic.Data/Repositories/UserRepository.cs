using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Automapper.Infrastructure;
using Clinic.Data.Context;

namespace Clinic.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IDataMapper _mapper;

        public UserRepository(
            IDataMapper mapper,
            ClinicDb context) : base(context)
        {
            _mapper = mapper;
        }
    }
}
