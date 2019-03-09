namespace ClinicApi.Models
{
    public class UpdatePropertyModel<T>
    {
        public int Id { get; set; }
        public T Value { get; set; }
    }
}