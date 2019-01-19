namespace ClinicApi.Models.Document
{
    public class DocumentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public int UserId { get; set; }
    }
}