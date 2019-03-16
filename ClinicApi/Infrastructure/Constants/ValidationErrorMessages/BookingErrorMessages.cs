﻿namespace ClinicApi.Infrastructure.Constants.ValidationErrorMessages
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

        public static string BadRateValue(float value)
        {
            return $"Rate should be less then {value} and greater then zero!";
        }
    }
}