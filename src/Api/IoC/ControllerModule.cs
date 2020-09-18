using System.Reflection;
using Autofac;
using Autofac.Features.AttributeFilters;
using Module = Autofac.Module;

namespace Api.IoC {
    public class ControllerModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("Controller"))
                .WithAttributeFiltering()
                .PropertiesAutowired();
        }
    }
}