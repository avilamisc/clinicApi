using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Core.DtoModels;
using Clinic.Core.Repositories;
using Clinic.Data.Automapper.Infrastructure;
using Clinic.Data.Context;

namespace Clinic.Data.Repositories
{
    public class ClinicRepository: Repository<Core.Entities.Clinic>, IClinicRepository
    {
        private readonly IDataMapper _mapper;

        public ClinicRepository(
            IDataMapper mapper,
            ClinicDb clinicDb) : base(clinicDb)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClinicDto>> GetAllClinicsAsync(PagingDto paging, DbGeography location)
        {
            var query = await _context.Clinics
               .OrderBy(c => c.Geolocation.Distance(location))
               .Skip(paging.PageSize * paging.PageNumber)
               .Take(paging.PageSize)
               .ToListAsync();

            return _mapper.Mapper.Map<IEnumerable<ClinicDto>>(query);
        }
    }
}
