using System.Threading;
using System.Threading.Tasks;
using Api.Data.Repositories.Database.Dapper;
using Api.Domain.Dto;
using Core.Domain.Validators;
using Core.Extensions;
using FluentValidation;
using FluentValidation.Validators;

namespace Api.Domain.Validators {
    public sealed class CreateAuditValidator : CreateValidator<CreateAuditDto> {
        private readonly IUserRepository userDbRepository;
        private readonly IAuditRepository auditDbRepository;

        public CreateAuditValidator(
            IUserRepository userDbRepository,
            IAuditRepository auditDbRepository
        ) {
            this.userDbRepository = userDbRepository;
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
                    "У пользователя за сутки уже отработано "
                    + "'{HoursSum}' ч. "
                    + "Нельзя добавить еще '{Hours}' ч. "
                    + "Сумма отработанных часов не может быть больше 24 ч."
                )
                .InclusiveBetween(1, 24)
                .NotNull();
            RuleFor(x => x.Date).NotNull();
            RuleFor(x => x.UserId)
                .NotNull()
                .ExistsAsync(userDbRepository);
        }

        private async Task<bool> BeValidHoursSum(
            CreateAuditDto inDto,
            int? hours,
            PropertyValidatorContext context,
            CancellationToken cancellationToken
        ) {
            if (inDto.Date is null
                || hours is null
                || inDto.UserId is null) return true;

            var hoursSum =
                await auditDbRepository.GetHoursSumByUserIdDateAsync(
                    userId: inDto.UserId.Value,
                    date: inDto.Date.Value.Date
                );
            context.MessageFormatter.AppendArgument(
                name: "HoursSum",
                value: hoursSum
            );
            context.MessageFormatter.AppendArgument(
                name: "Hours",
                value: hours
            );
            return hoursSum + hours <= 24;
        }
    }
}