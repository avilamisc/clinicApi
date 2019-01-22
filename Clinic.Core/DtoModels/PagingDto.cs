namespace Clinic.Core.DtoModels
{
    public class PagingDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
