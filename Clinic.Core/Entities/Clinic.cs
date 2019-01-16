using System.Collections.Generic;

namespace Clinic.Core.Entities
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ClinicClinician> ClinicClinicians { get; set; }

        public Clinic()
        {
            ClinicClinicians = new List<ClinicClinician>();
        }
    }
}
