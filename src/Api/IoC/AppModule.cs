using Autofac;

namespace Api.IoC {
    public class AppModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterModule<DatabaseModule>();
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule<RepositoryModule>();
            builder.RegisterModule<ControllerModule>();
            builder.RegisterModule<AutoMapperModule>();
            builder.RegisterModule<ValidatorModule>();
        }
    }
}