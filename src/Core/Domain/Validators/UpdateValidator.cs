using Core.Domain.Entities;
using FluentValidation;

namespace Core.Domain.Validators {
    public abstract class UpdateValidator<TEntity, TId, TDto>
        : AbstractValidator<TDto>
        where TEntity : IEntity<TId> {
        public TEntity EntityToUpdate { protected get; set; }
        protected abstract void ApplyRules();
    }
}