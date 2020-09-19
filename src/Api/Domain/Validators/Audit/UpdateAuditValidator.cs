using System.Threading;
using System.Threading.Tasks;
using Api.Data.Repositories.Database.Dapper;
using Api.Domain.Dto;
using Api.Domain.Entities;
using Core.Domain.Validators;
using Core.Extensions;
using FluentValidation;
using FluentValidation.Validators;

namespace Api.Domain.Validators {
    public sealed class UpdateAuditValidator :
        UpdateValidator<Audit, long, UpdateAuditDto> {
        private readonly IAuditRepository auditDbRepository;

        public UpdateAuditValidator(IAuditRepository auditDbRepository) {
            this.auditDbRepository = auditDbRepository;
            ApplyRules();
        }

        protected override void ApplyRules() {
            RuleFor(x => x.Description)
                .NotNull()
                .Length(1, 255)
                .NotOnlySpaces();
            RuleFor(x => x.Hours)
                .MustAsync(BeValidHoursSum)
                .WithMessage(
                    "У пользователя за сутки будет отработано "
                    + "'{NewHoursSum}' ч. "
                    + "Сумма отработанных часов не может быть больше 24 ч."
                )
                .InclusiveBetween(1, 24)
                .NotNull();
            RuleFor(x => x.Date).NotNull();
        }

        private async Task<bool> BeValidHoursSum(
            UpdateAuditDto inDto,
            int? hours,
            PropertyValidatorContext context,
            CancellationToken cancellationToken
        ) {
            if (hours is null || inDto.Date is null) return true;

            var hoursSum =
                await auditDbRepository.GetHoursSumByUserIdDateAsync(
                    userId: EntityToUpdate.User.Id,
                    date: inDto.Date.Value.Date
                );
            var currentHours = EntityToUpdate.Date == inDto.Date.Value.Date
                ? EntityToUpdate.Hours
                : 0;
            var newHoursSum = hoursSum - currentHours + hours;
            context.MessageFormatter.AppendArgument(
                name: "NewHoursSum",
                value: newHoursSum
            );
            return newHoursSum <= 24;
        }
    }
}