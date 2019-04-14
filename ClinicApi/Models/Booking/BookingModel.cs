using System;
using System.Collections.Generic;

using Clinic.Core.Enums;
using ClinicApi.Models.Document;

namespace ClinicApi.Models.Booking
{
    public class BookingModel
    {
        public int Id { get; set; }
        public Stage Stage { get; set; }
        public string Name { get; set; }
        public float? Rate { get; set; }
        public short? HeartRate { get; set; }
        public float? Weight { get; set; }
        public short? Height { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string PatientDescription { get; set; }
        public ICollection<DocumentModel> Documents { get; set; }

        private const float minHeartRateValue = 20;
        private const float maxHeartRateValue = 200;
        private const float maxRateValue = 5;

        public virtual string CheckValidationError()
        {
            if (HeartRate.HasValue &&
                (HeartRate.Value <= minHeartRateValue || HeartRate.Value >= maxHeartRateValue))
            {
                return $"Heart rate should be in range [{minHeartRateValue},{maxHeartRateValue}].";
            }

            if (Rate.HasValue && (Rate.Value < 0 || Rate.Value >= maxRateValue))
            {
                return $"Rate should be in range [{0},{maxRateValue}].";
            }

            if (Weight.HasValue && Weight < 0)
            {
                return $"Weight should be positive value.";
            }

            if (Height.HasValue && Height < 0)
            {
                return $"Height should be positive value.";
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                return $"Name is missed.";
            }

            return null;
        }
    }
}