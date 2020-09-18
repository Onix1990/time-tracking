using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Api.IoC {
    public class ValidatorModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("Validator"))
                .InstancePerDependency();
        }
    }
}