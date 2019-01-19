using Autofac;
using Autofac.Builder;
using Clinic.Core;
using Clinic.Core.Automapper;
using Clinic.Core.Enums;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Infrastructure.Constants;
using ClinicApi.Interfaces;
using ClinicApi.Services;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ClinicApi.Infrastructure
{
    public class AppModule : CoreModule
    {
        private readonly string _securityKey;
        private readonly int _accessTokenExpireMins;
        private readonly int _refreshTokenExpireDays;
        private readonly int _hashingComplexity;

        public AppModule(DILifetimeType dILifetimeType) : base(dILifetimeType)
        {
            _securityKey = ConfigurationManager.AppSettings[ConfigKeyConstants.AuthSecretKey];
            _accessTokenExpireMins = Int32.Parse(ConfigurationManager.AppSettings[ConfigKeyConstants.AccessTokenExpire]);
            _refreshTokenExpireDays = Int32.Parse(ConfigurationManager.AppSettings[ConfigKeyConstants.RefreshTokenExpire]);
            _hashingComplexity = Int32.Parse(ConfigurationManager.AppSettings[ConfigKeyConstants.HashingComplexity]);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegistrationBuilders = new List<IRegistrationBuilder<object, object, object>>
            {
                builder.Register(c => new AppSettings
                {
                    AuthSecret = _securityKey,
                    AccessTokenExpirtionTime = _accessTokenExpireMins,
                    RefreshTokenExpirationDays = _refreshTokenExpireDays,
                    WorkFactorComplexity = _hashingComplexity
                }).AsSelf(),

                builder.Register(c => new ApiMapper(ApiMapper.GetMapperConfiguration().CreateMapper())).As<IApiMapper>(),

                builder.RegisterType<TokenService>().As<ITokenService>(),
                builder.RegisterType<BookingService>().As<IBookingService>(),
                builder.RegisterType<AccountService>().As<IAccountService>(),
                builder.RegisterType<FileService>().As<IFileService>(),
                builder.RegisterType<ClinicService>().As<IClinicService>(),
                builder.RegisterType<ClinicianService>().As<IClinicianService>()
            };

            SetDependenciesInstanceType();
        }
    }
}