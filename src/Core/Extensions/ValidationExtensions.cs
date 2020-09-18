using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;

namespace Core.Extensions {
    public static class ValidationExtensions {
        public static IDictionary<string, string[]> FormatValidationErrors(
            this IEnumerable<ValidationFailure> validationFailures
        ) {
            return validationFailures
                   .GroupBy(x => x.PropertyName)
                   .ToDictionary(x => x.Key,
                                 group =>
                                     group.Select(x => x.ErrorMessage)
                                          .ToArray()
                   );
        }

        public static IRuleBuilderOptions<TDto, string> NotOnlySpaces<TDto>(
            this IRuleBuilder<TDto, string> ruleBuilder
        ) {
            return ruleBuilder
                   .Must(
                       x => {
                           if (string.IsNullOrEmpty(x)) {
                               return true;
                           }

                           return !x.All(char.IsWhiteSpace);
                       }
                   )
                   .WithMessage(
                       "Поле не должно содержать только символы пробелов"
                   );
        }

        public static IRuleBuilderInitial<TDto, TInputId> ExistsAsync<
            TEntity, TId, TInputId, TDto>(
            this IRuleBuilder<TDto, TInputId> ruleBuilder,
            ICrudRepository<TEntity, TId> dbRepository
        ) where TEntity : IEntity<TId> {
            async Task ExistsAsync(
                TInputId id,
                CustomContext context,
                CancellationToken cancellationToken
            ) {
                var underlyingType =
                    Nullable.GetUnderlyingType(typeof(TInputId));
                var type = underlyingType != null
                    ? underlyingType
                    : typeof(TInputId);

                if (type != typeof(TId) || id == null) return;

                var typedId = (TId) Convert.ChangeType(id, typeof(TId));
                var entity = await dbRepository.GetByIdAsync(typedId)
                                               .ConfigureAwait(false);

                if (entity == null) {
                    context.AddFailure("Объект не найден");
                }
            }

            return ruleBuilder.CustomAsync(ExistsAsync);
        }
    }
}