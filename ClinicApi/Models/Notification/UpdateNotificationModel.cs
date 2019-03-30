namespace ClinicApi.Models.Notification
{
    public class UpdateNotificationModel : CreateNotificationModel
    {
        public bool? IsRead { get; set; }
        public int Id { get; set; }

        public override string Validate()
        {
            if (!IsRead.HasValue)
            {
                return "Please set notification read state.";
            }

            return base.Validate();
        }
    }
}