using Api.Domain.Dto;
using Api.Domain.Entities;
using Core.Domain.Services;

namespace Api.Domain.Services {
    public interface IUserService :
        ICrudService<
            User, long,
            CreateUserDto, UpdateUserDto, UserOutDto
        > { }
}