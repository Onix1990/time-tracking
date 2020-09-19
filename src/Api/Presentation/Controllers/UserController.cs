using System.Data;
using System.Threading.Tasks;
using Api.Common.ActionFilters;
using Api.Domain.Dto;
using Api.Domain.Entities;
using Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Presentation.Controllers {
    public class UserController :
        BaseCrudController<
            IUserService,
            User, long,
            CreateUserDto, UpdateUserDto, UserOutDto
        > {
        public UserController(IUserService service) : base(service) { }

        [UowType(IsolationLevel.Serializable)]
        public override Task<ActionResult<UserOutDto>> CreateAsync(
            CreateUserDto inDto
        ) {
            return base.CreateAsync(inDto);
        }

        [UowType(IsolationLevel.Serializable)]
        public override Task<ActionResult<UserOutDto>> UpdateAsync(
            long id, UpdateUserDto inDto
        ) {
            return base.UpdateAsync(id, inDto);
        }
    }
}