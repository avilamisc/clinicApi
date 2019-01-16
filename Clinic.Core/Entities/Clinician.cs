using System.Collections.Generic;

namespace Clinic.Core.Entities

{
    public class Clinician : User
    {
        public int Rate { get; set; }

        public ICollection<ClinicClinician> ClinicClinicians { get; set; }

        public Clinician()
        {
            ClinicClinicians = new List<ClinicClinician>();
        }
    }
}
