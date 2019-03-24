using System;

namespace ClinicApi.Models.Notification
{
    public class NotificationModel
    {
        public string Message { get; set; }
        public int Id { get; set; }
        public bool IsRead { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string UserMail { get; set; }
    }
}