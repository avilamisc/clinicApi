using System;

namespace ClinicApi.Models.Account.Registration
{
    public class PatientRegisterModel: UserRegisterModel
    {
        public DateTime BornDate { get; set; }
    }
}