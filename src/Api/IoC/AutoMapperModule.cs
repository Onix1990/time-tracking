using Api.Domain.MappingProfiles;
using Autofac;
using AutoMapper;

namespace Api.IoC {
    public class AutoMapperModule : Module {
        protected override void Load(ContainerBuilder builder) {
            var mapperConfiguration = new MapperConfiguration(
                cfg => cfg.AddProfile<DefaultMappingProfile>()
            );

            builder
                .RegisterInstance(mapperConfiguration.CreateMapper())
                .As<IMapper>()
                .SingleInstance();
        }
    }
}