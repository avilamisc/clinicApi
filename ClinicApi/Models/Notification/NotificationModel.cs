using System;

namespace ClinicApi.Models.Notification
{
    public class NotificationModel
    {
        public string Message { get; set; }
        public int Id { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreationDate { get; set; }
    }
}