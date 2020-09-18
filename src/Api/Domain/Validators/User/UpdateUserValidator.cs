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
    public sealed class UpdateUserValidator :
        UpdateValidator<User, long, UpdateUserDto> {
        private readonly IUserRepository userDbRepository;

        public UpdateUserValidator(IUserRepository userDbRepository) {
            this.userDbRepository = userDbRepository;
            ApplyRules();
        }

        protected override void ApplyRules() {
            RuleFor(x => x.FirstName)
                .NotNull()
                .Length(1, 100)
                .NotOnlySpaces();
            RuleFor(x => x.LastName)
                .NotNull()
                .Length(1, 100)
                .NotOnlySpaces();
            RuleFor(x => x.Patronymic)
                .NotNull()
                .Length(0, 100)
                .NotOnlySpaces();
            RuleFor(x => x.Email)
                .MustAsync(BeUniqueEmail)
                .WithMessage("Пользователь с email '{Email}' уже существует")
                .When(x => x.Email != null)
                .NotNull()
                .EmailAddress();
        }

        private async Task<bool> BeUniqueEmail(
            UpdateUserDto inDto,
            string email,
            PropertyValidatorContext context,
            CancellationToken cancellationToken
        ) {
            context.MessageFormatter.AppendArgument("Email", email);
            var user = await userDbRepository.GetByEmailAsync(email);
            return user is null || user == EntityToUpdate;
        }
    }
}