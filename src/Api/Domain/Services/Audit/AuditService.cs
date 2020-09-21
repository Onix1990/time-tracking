using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Data.Repositories.Database.Dapper;
using Api.Domain.Dto;
using Api.Domain.Entities;
using Api.Domain.Validators;
using AutoMapper;
using Core.Domain.Services;
using FluentValidation;

namespace Api.Domain.Services {
    public class AuditService :
        CrudService<
            Audit, long,
            IAuditRepository,
            CreateAuditDto, UpdateAuditDto, AuditOutDto,
            CreateAuditValidator, UpdateAuditValidator
        >,
        IAuditService {
        private readonly IUserRepository userDbRepository;

        public AuditService(
            IAuditRepository repository,
            CreateAuditValidator createValidator,
            UpdateAuditValidator updateValidator,
            IMapper mapper, IUserRepository userDbRepository) :
            base(repository, mapper, createValidator, updateValidator) {
            this.userDbRepository = userDbRepository;
        }

        public async Task<IList<AuditOutDto>> GetAllByAsync(
            int? year,
            int? month,
            long? userId
        ) {
            var audits = await Repository.GetAllByAsync(year, month, userId);
            return Mapper.Map<IList<AuditOutDto>>(audits);
        }

        public override async Task<AuditOutDto> CreateAsync(
            CreateAuditDto inDto
        ) {
            await CreateValidator.ValidateAndThrowAsync(inDto);

            var audit = Mapper.Map<Audit>(inDto);

            audit.User = await userDbRepository.GetByIdAsync(
                id: inDto.UserId.Value
            );
            await Repository.SaveAsync(audit);

            return Mapper.Map<AuditOutDto>(audit);
        }
    }
}