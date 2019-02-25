using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace Clinic.Core.Entities
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public DbGeography Geolocation { get; set; }

        public ICollection<ClinicClinician> ClinicClinicians { get; set; }

        public Clinic()
        {
            ClinicClinicians = new List<ClinicClinician>();
        }
    }
}
