using Api.Domain.Dto;
using Api.Domain.Entities;
using Core.Domain.Validators;
using Core.Extensions;
using FluentValidation;

namespace Api.Domain.Validators {
    public sealed class UpdateAuditValidator :
        UpdateValidator<Audit, long, UpdateAuditDto> {
        public UpdateAuditValidator() {
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
        }
    }
}