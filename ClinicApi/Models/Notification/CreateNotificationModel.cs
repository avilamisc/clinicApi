using System;

namespace ClinicApi.Models.Notification
{
    public class CreateNotificationModel
    {
        public string Content { get; set; }
        public DateTime? CreationDate { get; set; }
        public int UserId { get; set; }

        public virtual string Validate()
        {
            if (string.IsNullOrWhiteSpace(Content))
            {
                return "Content is required";
            }

            if (CreationDate == null)
            {
                return "Creation date is required";
            }

            return null;
        }
    }
}