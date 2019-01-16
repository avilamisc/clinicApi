using Clinic.Core.Entities;
using Clinic.Core.DtoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinic.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<DocumentDto>> GetUserDocumentDtosAsync(int id);
    }
}
