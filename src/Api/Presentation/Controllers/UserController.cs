using Api.Domain.Dto;
using Api.Domain.Entities;
using Api.Domain.Services;

namespace Api.Presentation.Controllers {
    public class UserController :
        BaseCrudController<
            IUserService,
            User, long,
            CreateUserDto, UpdateUserDto, UserOutDto
        > {
        public UserController(IUserService service) : base(service) { }
    }
}