using System.Collections.Generic;

namespace ClinicApi.Models
{
    public class PagingResult<T>
    {
        public IEnumerable<T> DataCollection { get; set; }
        public int TotalCount { get; set; }
    }
}