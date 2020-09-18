using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Dto;
using Api.Domain.Entities;
using Core.Domain.Services;

namespace Api.Domain.Services {
    public interface IAuditService :
        ICrudService<
            Audit, long,
            CreateAuditDto, UpdateAuditDto, AuditOutDto
        > {
        Task<IList<AuditOutDto>> GetAllByAsync(
            int? year,
            int? month,
            long? userId
        );
    }
}