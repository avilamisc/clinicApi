using Autofac;
using Autofac.Integration.WebApi;
using Clinic.Core;
using Clinic.Core.Enums;
using Clinic.Data;
using ClinicApi.Infrastructure;
using ClinicApi.Infrastructure.Constants;
using SqlServerTypes;
using System.Configuration;
using System.Data.Entity.SqlServer;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace ClinicApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            SqlProviderServices.SqlServerTypesAssemblyName =
                "Microsoft.SqlServer.Types, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91";

            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule(new DataModule(
                DILifetimeType.PerRequest,
                ConfigurationManager.ConnectionStrings[ApiConstants.ConnectionStirngName].ConnectionString));
            builder.RegisterModule(new CoreModule(DILifetimeType.PerRequest));
            builder.RegisterModule(new AppModule(DILifetimeType.PerRequest));

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
