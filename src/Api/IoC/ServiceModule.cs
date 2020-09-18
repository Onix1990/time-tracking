using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Api.IoC {
    public class ServiceModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}