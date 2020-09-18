using System.Reflection;
using Autofac;
using Autofac.Features.AttributeFilters;
using Module = Autofac.Module;

namespace Api.IoC {
    public class RepositoryModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .WithAttributeFiltering()
                .InstancePerLifetimeScope();
        }
    }
}