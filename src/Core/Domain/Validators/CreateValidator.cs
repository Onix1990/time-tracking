using FluentValidation;

namespace Core.Domain.Validators {
    public abstract class CreateValidator<TDto> : AbstractValidator<TDto> {
        protected abstract void ApplyRules();
    }
}