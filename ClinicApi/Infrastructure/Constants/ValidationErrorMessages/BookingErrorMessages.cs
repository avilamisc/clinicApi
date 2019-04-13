namespace ClinicApi.Infrastructure.Constants.ValidationErrorMessages
{
    public static class BookingErrorMessages
    {
        public const string WrongBookingDataFormat = "Wrong booking data format";
        public const string MissedFile = "Missed or unsupported file";
        public const string UpdateError = "Cannot update this item";
        public const string ValidationDataError = "Wrong booking data";
        public const string MissedClinicClinician = "Wrong booking data";
        public const string UnexistingBooking = "Such booking doesn`t exist!";
        public const string PermissionsToDelete = "You don`t have permissions to delete this.";
        public const string SuccessfulDelete = "Successfuly delete booking";
        public const string AccessIsDenied = "Access is denied";
        public const string CannotUpdateNotSendBooking = "Booking mst be in send stage";
        public const string CannotUpdateNotSendOrConfirmedBooking = "Booking mst be in send or confirmed stage";
        public const string CannotUpdateNotConfirmedOrInProgressBooking = "Booking mst be in send or in-progress stage";
        public const string CannotUpdateNotInProgressBooking = "Booking mst be in progress stage";
        public const string CannotRateNotCompletedBooking = "Booking should be completed to rate it.";

        public static string BadRateValue(float value)
        {
            return $"Rate should be less then {value} and greater then zero!";
        }
    }
}