using AutoMapper;
using Clinic.Core.Automapper;
using System;

namespace ClinicApi.Automapper.Infrastructure
{
    public class ApiMapper : CoreMapperWrapper, IApiMapper
    {
        public ApiMapper(IMapper mapper) : base(mapper)
        {
        }

        public TEntity SafeMap<TEntity>(object source) where TEntity : class
        {
            try
            {
                return _mapper.Map<TEntity>(source);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static MapperConfiguration GetMapperConfiguration()
        {
            return new MapperConfiguration(
                config =>
                {
                    config.AddProfile(new BookingProfile());
                    config.AddProfile(new DocumentProfile());
                    config.AddProfile(new PaginationProfile());
                    config.AddProfile(new ClinicianProfile());
                    config.AddProfile(new ClinicProfile());
                    config.AddProfile(new AccountProfile());
                    config.AddProfile(new NotificationProfile());
                });
        }
    }
}