using Autofac;
using Clinic.Core.Automapper;
using Clinic.Core.UnitOfWork;
using Clinic.Core.Repositories;
using Clinic.Data.Context;
using Clinic.Domain.Repositories.Concrete;
using Clinic.Core;
using System.Collections.Generic;
using Autofac.Builder;
using Clinic.Core.Enums;
using Clinic.Data.Automapper.Infrastructure;

namespace Clinic.Data
{
    public class DataModule : CoreModule
    {
        private readonly string _connectionString;

        public DataModule(
            DILifetimeType dILifetimeType,
            string connectionString) : base(dILifetimeType)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegistrationBuilders = new List<IRegistrationBuilder<object, object, object>>
            {
                builder.Register(c => new DataMapper(DataMapper.GetMapperConfiguration().CreateMapper())).As<IDataMapper>(),

                builder.Register(c => new ClinicDb(_connectionString)).AsSelf(),
                builder.RegisterType<BookingRepository>().As<IBookingRepository>(),
                builder.RegisterType<ClinicClinicianRepository>().As<IClinicClinicianRepository>(),
                builder.RegisterType<RefreshTokenRepository>().As<IRefreshTokenRepository>(),
                builder.RegisterType<UserRepository>().As<IUserRepository>(),
                builder.RegisterType<UnitOfWork.UnitOfWork>().As<IUnitOfWork>()
            };

            SetDependenciesInstanceType();
        }
    }
}
