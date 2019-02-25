using System.Collections.Generic;

namespace ClinicApi.Models.Account.Registration
{
    public class ClinicianRegisterModel: UserRegisterModel
    {
        public IEnumerable<int> ClinicsId { get; set; }
    }
}