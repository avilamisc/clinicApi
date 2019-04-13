using System;

namespace Clinic.Core.DtoModels
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BornDate { get; set; }
    }
}
