using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Api.Common.ActionFilters;
using Api.Domain.Dto;
using Api.Domain.Entities;
using Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Presentation.Controllers {
    public class AuditController :
        BaseCrudController<
            IAuditService,
            Audit, long,
            CreateAuditDto, UpdateAuditDto, AuditOutDto
        > {
        public AuditController(IAuditService service) : base(service) { }

        [HttpGet]
        [UowType]
        public async Task<ActionResult<IList<AuditOutDto>>> GetAllByAsync(
            [FromQuery] int? year,
            [FromQuery] int? month,
            [FromQuery] long? userId
        ) {
            var outDtos = await Service.GetAllByAsync(
                year: year,
                month: month,
                userId: userId
            );
            return Ok(outDtos);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public override Task<ActionResult<IList<AuditOutDto>>> GetAllAsync() {
            throw new NotSupportedException();
        }

        [UowType(IsolationLevel.Serializable)]
        public override Task<ActionResult<AuditOutDto>> CreateAsync(
            CreateAuditDto inDto
        ) {
            return base.CreateAsync(inDto);
        }

        [UowType(IsolationLevel.Serializable)]
        public override Task<ActionResult<AuditOutDto>> UpdateAsync(
            long id,
            UpdateAuditDto inDto
        ) {
            return base.UpdateAsync(id, inDto);
        }
    }
}