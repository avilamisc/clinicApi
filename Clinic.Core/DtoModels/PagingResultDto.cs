using System.Collections.Generic;

namespace Clinic.Core.DtoModels
{
    public class PagingResultDto<TEntity>
    {
        public IEnumerable<TEntity> DataColection { get; set; }
        public int TotalCount { get; set; }
    }
}
