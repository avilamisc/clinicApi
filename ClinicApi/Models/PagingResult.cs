namespace ClinicApi.Models
{
    public class PagingResult<T>
    {
        public T Result { get; set; }
        public int TotalAmount { get; set; }
    }
}