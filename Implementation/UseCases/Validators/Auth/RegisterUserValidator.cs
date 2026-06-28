using Application.DTO.Auth;
using FluentValidation;

namespace Implementation.UseCases.Validators.Auth
{
    public class RegisterUserValidator : BaseValidator<RegisterUserDTO>
    {
        public RegisterUserValidator(AppDbContext ctx)
        {
            this.RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Required)
            .Matches("^[A-Za-z0-9]+(?:[ _-][A-Za-z0-9]+)*$")
            .WithMessage("Username can only contain letters, numbers, and underscores.")
            .Must(username => !ctx.Users.Any(u => u.Username == username))
            .WithMessage("Username is already in use.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(Required)
                .MinimumLength(3).WithMessage("First name must be at least 3 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(Required)
                .MinimumLength(3).WithMessage("Last name must be at least 3 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Required)
                .EmailAddress().WithMessage("Invalid email format.")
                .Must(email => !ctx.Users.Any(u => u.Email == email))
                .WithMessage("Email is already in use.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(Required)
                .Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$")
                .WithMessage("Password must be at least 8 characters and contain at least one uppercase letter and one number.");
        }
    }
}
