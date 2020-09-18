using Api.Data.Repositories.Database.Dapper;
using Api.Domain.Dto;
using Core.Domain.Validators;
using Core.Extensions;
using FluentValidation;

namespace Api.Domain.Validators {
    public sealed class CreateAuditValidator : CreateValidator<CreateAuditDto> {
        private readonly IUserRepository userDbRepository;

        public CreateAuditValidator(IUserRepository userDbRepository) {
            this.userDbRepository = userDbRepository;
            ApplyRules();
        }

        protected override void ApplyRules() {
            RuleFor(x => x.Description)
                .NotNull()
                .Length(1, 255)
                .NotOnlySpaces();
            RuleFor(x => x.Hours)
                .NotNull()
                .InclusiveBetween(0, 24);
            RuleFor(x => x.Date).NotNull();
            RuleFor(x => x.UserId)
                .NotNull()
                .ExistsAsync(userDbRepository);
        }
    }
}