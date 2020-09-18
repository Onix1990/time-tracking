using Api.Data.Repositories.Database.Dapper;
using Api.Domain.Dto;
using Api.Domain.Entities;
using Api.Domain.Validators;
using AutoMapper;
using Core.Domain.Services;

namespace Api.Domain.Services {
    public class UserService :
        CrudService<
            User, long,
            IUserRepository,
            CreateUserDto, UpdateUserDto, UserOutDto,
            CreateUserValidator, UpdateUserValidator
        >,
        IUserService {
        public UserService(
            IUserRepository repository,
            CreateUserValidator createValidator,
            UpdateUserValidator updateValidator,
            IMapper mapper
        ) : base(repository, mapper, createValidator, updateValidator) { }
    }
}