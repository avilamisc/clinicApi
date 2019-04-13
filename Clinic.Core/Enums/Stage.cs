namespace Clinic.Core.Enums
{
    public enum Stage
    {
        Send = 1, // Patient send it but clinician didn`t approved it yet
        Confirmed, // Clinician confirm it but haven`t sart working
        InProgress, // Clinician set up all patients props
        Rejected, // Clinician reject it at send stage
        Canceled, // Clinician or patient has cancelled it
        Completed // Clinician finish and set up Clinician decission property
    }
}
