using Application.DTO.Users;
using FluentValidation;

namespace Implementation.UseCases.Validators.Users
{
    public class UpdateUserValidator : BaseValidator<UpdateUserDTO>
    {
        public UpdateUserValidator(AppDbContext ctx)
        {
            this.RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Required)
            .Matches("^[A-Za-z0-9]+(?:[ _-][A-Za-z0-9]+)*$")
            .WithMessage("Username can only contain letters, numbers, and underscores.")
            .Must((dto, username) => !ctx.Users.Any(u => u.Username == username && u.Id != dto.Id))
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
                .Must((dto, email) => !ctx.Users.Any(u => u.Email == email && u.Id != dto.Id))
                .WithMessage("Email is already in use.");

            RuleFor(x => x.RoleId)
                .Must(id => ctx.Roles.Any(r => r.Id == id))
                .WithMessage("Invalid role.");
        }
    }
}
