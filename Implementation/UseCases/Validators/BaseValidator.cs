using FluentValidation;

namespace Implementation.UseCases.Validators
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        protected const string Required = "Field is required.";
    }
}
