using Autofac;
using Autofac.Builder;
using Clinic.Core.Automapper;
using Clinic.Core.Enums;
using System.Collections.Generic;

namespace Clinic.Core
{
    public class CoreModule : Module
    {
        protected readonly DILifetimeType LifetimeType;
        protected List<IRegistrationBuilder<object, object, object>> RegistrationBuilders;

        public CoreModule(DILifetimeType DILifetimeType)
        {
            LifetimeType = DILifetimeType;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegistrationBuilders = new List<IRegistrationBuilder<object, object, object>>
            {
                builder.RegisterType<CoreMapperWrapper>().As<ICoreMapper>()
            };

            SetDependenciesInstanceType();
        }

        protected void SetDependenciesInstanceType()
        {
            foreach (var dependency in RegistrationBuilders)
            {
                switch (LifetimeType)
                {
                    case DILifetimeType.PerRequest:
                        dependency.InstancePerRequest(); break;
                    case DILifetimeType.PerDependency:
                        dependency.InstancePerDependency(); break;
                    case DILifetimeType.SingleInstance:
                        dependency.SingleInstance(); break;
                }
            }
        }
    }
}
