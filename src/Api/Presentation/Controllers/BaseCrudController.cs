using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Api.Common.ActionFilters;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Domain.Services;
using Core.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Presentation.Controllers {
    [ApiController]
    [Route("api/v1/[controller]s/")]
    public abstract class BaseCrudController<
        TService,
        TEntity, TId,
        TCreateDto, TUpdateDto, TOutDto
    > : ControllerBase
        where TService : ICrudService<
            TEntity, TId, 
            TCreateDto, TUpdateDto, TOutDto
        >
        where TEntity : Entity<TId> {
        protected TService Service { get; }

        protected BaseCrudController(TService service) {
            Service = service;
        }

        [HttpGet]
        [UowType]
        public virtual async Task<ActionResult<IList<TOutDto>>> GetAllAsync() {
            var outDtos = await Service.GetAllAsync();
            return Ok(outDtos);
        }

        [HttpGet("{id}")]
        [UowType]
        public virtual async Task<ActionResult<TOutDto>> GetByIdAsync(
            [FromRoute] TId id
        ) {
            try {
                var outDto = await Service.GetByIdAsync(id);
                return Ok(outDto);
            }
            catch (ObjectNotFoundException) {
                return NotFound();
            }
        }

        [HttpPost]
        [UowType]
        [Consumes("application/json")]
        public virtual async Task<ActionResult<TOutDto>> CreateAsync(
            [FromBody] TCreateDto inDto
        ) {
            try {
                var outDto = await Service.CreateAsync(inDto);
                return Created(Request.Path, outDto);
            }
            catch (ValidationException e) {
                return BadRequest(
                    error: new ValidationProblemDetails(
                        e.Errors.FormatValidationErrors()
                    )
                );
            }
        }

        [HttpPut("{id}")]
        [Consumes("application/json")]
        [UowType(IsolationLevel.RepeatableRead)]
        public virtual async Task<ActionResult<TOutDto>> UpdateAsync(
            [FromRoute] TId id,
            [FromBody] TUpdateDto inDto
        ) {
            try {
                var outDto = await Service.UpdateAsync(id, inDto);
                return Ok(outDto);
            }
            catch (ObjectNotFoundException) {
                return NotFound();
            }
            catch (ValidationException e) {
                return BadRequest(
                    error: new ValidationProblemDetails(
                        e.Errors.FormatValidationErrors()
                    )
                );
            }
        }

        [HttpDelete("{id}")]
        [UowType(IsolationLevel.RepeatableRead)]
        public virtual async Task<ActionResult>
            DeleteAsync([FromRoute] TId id) {
            try {
                await Service.DeleteAsync(id);
                return NoContent();
            }
            catch (ObjectNotFoundException) {
                return NotFound();
            }
        }
    }
}