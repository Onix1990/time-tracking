using System.Data;
using Api.Data.Database;
using Autofac;
using Core.Data;

namespace Api.IoC {
    public class DatabaseModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder
                .RegisterType<DapperDatabase>()
                .As<IDatabase<IDbConnection>>()
                .SingleInstance();

            builder
                .RegisterType<DapperUnitOfWork>()
                .As<IUnitOfWork<IDbConnection>>()
                .InstancePerLifetimeScope();
        }
    }
}