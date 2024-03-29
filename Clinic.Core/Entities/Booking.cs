﻿using System;
using System.Collections.Generic;

using Clinic.Core.Enums;

namespace Clinic.Core.Entities
{
    public class Booking : Entity
    {
        public string Name { get; set; }
        public float? Rate { get; set; }
        public short? HeartRate { get; set; }
        public float? Weight { get; set; }
        public short? Height { get; set; }
        public string PatientDescription { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Stage Stage { get; set; }

        public int ClinicClinicianId { get; set; }
        public ClinicClinician ClinicClinician { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public ICollection<Document> Documents { get; set; }
    }
}
