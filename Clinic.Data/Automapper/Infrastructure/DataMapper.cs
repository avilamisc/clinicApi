using AutoMapper;
using Clinic.Core.Automapper;

namespace Clinic.Data.Automapper.Infrastructure
{
    public class DataMapper : CoreMapperWrapper, IDataMapper
    {
        public DataMapper(IMapper mapper) : base(mapper)
        {
        }

        public static MapperConfiguration GetMapperConfiguration()
        {
            return new MapperConfiguration(
                config =>
                {
                    config.AddProfile(new BookingProfile());
                    config.AddProfile(new ClinicClinicianProfile());
                    config.AddProfile(new ClinicianProfile());
                    config.AddProfile(new ClinicProfile());
                    config.AddProfile(new DocumentProfile());
                    config.AddProfile(new PatientProfile());
                    config.AddProfile(new RefreshTokenProfile());
                    config.AddProfile(new UserProfile());
                });
        }
    }
}
