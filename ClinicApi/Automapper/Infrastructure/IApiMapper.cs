using Clinic.Core.Automapper;

namespace ClinicApi.Automapper.Infrastructure
{
    public interface IApiMapper : ICoreMapper
    {
        TEntity SafeMap<TEntity>(object source) where TEntity : class;
    }
}
