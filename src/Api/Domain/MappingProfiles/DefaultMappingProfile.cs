using Api.Domain.Dto;
using Api.Domain.Entities;
using AutoMapper;

namespace Api.Domain.MappingProfiles {
    public class DefaultMappingProfile : Profile {
        public DefaultMappingProfile() {
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UserOutDto>();
            CreateMap<CreateAuditDto, Audit>()
                .ForMember(
                    x => x.Date,
                    opt => opt.MapFrom(
                        x => x.Date != null ? x.Date.Value.Date : default
                    )
                );
            CreateMap<UpdateAuditDto, Audit>();
            CreateMap<Audit, AuditOutDto>();
        }
    }
}