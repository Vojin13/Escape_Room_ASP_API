using Application.DTO.Auth;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using Implementation.Emails;
using Implementation.UseCases.Commands.Auth;
using Implementation.UseCases.Validators.Auth;
using Tests.Common;
using Xunit;

namespace Tests.UseCases.Auth
{
    public class RegisterTests : TestBase
    {
        private EfRegisterUserCommand Command => new EfRegisterUserCommand(
            Ctx,
            new RegisterUserValidator(Ctx),
            new NoOpEmailSender(),
            new EmailTemplateComposer());

        private Role CreateUserRole()
        {
            var role = new Role { Name = "User", Slug = "user" };
            Ctx.Roles.Add(role);
            Ctx.SaveChanges();

            return role;
        }

        private RegisterUserDTO ValidRequest() => new RegisterUserDTO
        {
            Username = $"registeruser_{Guid.NewGuid():N}",
            FirstName = "Register",
            LastName = "Test",
            Email = $"register_{Guid.NewGuid():N}@example.com",
            Password = "Password123!"
        };

        [Fact]
        public void Execute_Throws_ValidationException_When_UsernameIsAlreadyInUse()
        {
            CreateUserRole();
            var existing = ValidRequest();
            Command.Execute(existing);

            var duplicate = ValidRequest();
            duplicate.Username = existing.Username;

            var act = () => Command.Execute(duplicate);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Execute_Throws_ValidationException_When_EmailIsAlreadyInUse()
        {
            CreateUserRole();
            var existing = ValidRequest();
            Command.Execute(existing);

            var duplicate = ValidRequest();
            duplicate.Email = existing.Email;

            var act = () => Command.Execute(duplicate);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Execute_Throws_ValidationException_When_PasswordIsWeak()
        {
            CreateUserRole();
            var request = ValidRequest();
            request.Password = "weak";

            var act = () => Command.Execute(request);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Execute_CreatesUser_When_DataIsValid()
        {
            var role = CreateUserRole();
            Ctx.RoleUseCases.Add(new RoleUseCase { RoleId = role.Id, UseCaseId = "some-use-case" });
            Ctx.SaveChanges();

            var request = ValidRequest();

            Command.Execute(request);

            var user = Ctx.Users.Single(x => x.Email == request.Email);

            user.Username.Should().Be(request.Username);
            user.RoleId.Should().Be(role.Id);
            user.EmailVerificationToken.Should().NotBeNull();
            BCrypt.Net.BCrypt.Verify(request.Password, user.Password).Should().BeTrue();

            Ctx.UserUseCases.Should().Contain(uc => uc.UserId == user.Id && uc.UseCaseId == "some-use-case");
        }
    }
}
