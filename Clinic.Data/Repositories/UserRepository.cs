using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Automapper.Infrastructure;
using Clinic.Data.Context;
using Clinic.Domain.Repositories.Base;

namespace Clinic.Domain.Repositories.Concrete
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

        public async Task<IEnumerable<DocumentDto>> GetUserDocumentDtosAsync(int id)
        {
            var user = await _context.Users
                .Include(u=> u.Documents)
                .SingleOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            return _mapper.Mapper.Map<List<DocumentDto>>(user.Documents);
        }
    }
}
