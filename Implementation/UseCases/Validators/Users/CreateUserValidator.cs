using Application.DTO.Users;
using FluentValidation;

namespace Implementation.UseCases.Validators.Users
{
    public class CreateUserValidator : BaseValidator<CreateUserDTO>
    {
        public CreateUserValidator(AppDbContext ctx)
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

            RuleFor(x => x.RoleId)
                .Must(id => ctx.Roles.Any(r => r.Id == id))
                .WithMessage("Invalid role.");
        }
    }
}
