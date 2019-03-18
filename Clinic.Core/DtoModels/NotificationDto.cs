using System;

namespace Clinic.Core.DtoModels
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
    }
}
