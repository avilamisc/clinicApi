namespace ClinicApi.Models
{
    public class RemoveResult<T>
    {
        public T Value { get; set; }
        public bool IsRemoved { get; set; }
        public string Description { get; set; }

        public static RemoveResult<T> Removed(string msg, T data) =>
            new RemoveResult<T> { IsRemoved = true, Description = msg, Value = data };

        public static RemoveResult<T> Failed(string msg) =>
            new RemoveResult<T> { IsRemoved = false, Description = msg };
    }
}