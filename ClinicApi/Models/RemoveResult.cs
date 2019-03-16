namespace ClinicApi.Models
{
    public class RemoveResult
    {
        public bool IsRemoved { get; set; }
        public string Description { get; set; }

        public static RemoveResult Removed(string msg) =>
            new RemoveResult { IsRemoved = true, Description = msg };

        public static RemoveResult Failed(string msg) =>
            new RemoveResult { IsRemoved = false, Description = msg };
    }
}