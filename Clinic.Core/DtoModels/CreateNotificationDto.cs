using System;

namespace Clinic.Core.DtoModels
{
    public class CreateNotificationDto
    {
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
        public int AuthorId { get; set; }
    }
}
