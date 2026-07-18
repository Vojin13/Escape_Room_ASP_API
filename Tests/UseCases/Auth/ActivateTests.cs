using Application.Exceptions;
using Domain.Entities;
using FluentAssertions;
using Implementation.Emails;
using Implementation.UseCases.Commands.Auth;
using Tests.Common;
using Xunit;

namespace Tests.UseCases.Auth
{
    public class ActivateTests : TestBase
    {
        private EfActivateAccountCommand Command => new EfActivateAccountCommand(
            Ctx,
            new NoOpEmailSender(),
            new EmailTemplateComposer());

        private User CreateTestUser(Guid? verificationToken, DateTime? emailVerifiedAt = null)
        {
            var role = new Role { Name = "User", Slug = "user" };
            Ctx.Roles.Add(role);
            Ctx.SaveChanges();

            var user = new User
            {
                Username = $"activateuser_{Guid.NewGuid():N}",
                FirstName = "Activate",
                LastName = "Test",
                Email = $"activate_{Guid.NewGuid():N}@example.com",
                Password = "hashed",
                RoleId = role.Id,
                EmailVerificationToken = verificationToken,
                EmailVerifiedAt = emailVerifiedAt
            };
            Ctx.Users.Add(user);
            Ctx.SaveChanges();

            return user;
        }

        [Fact]
        public void Execute_Throws_NotFoundException_When_CodeIsNotAValidGuid()
        {
            var act = () => Command.Execute("not-a-guid");

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void Execute_Throws_NotFoundException_When_NoUserMatchesCode()
        {
            var act = () => Command.Execute(Guid.NewGuid().ToString());

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void Execute_Throws_NotFoundException_When_AccountIsAlreadyVerified()
        {
            var token = Guid.NewGuid();
            CreateTestUser(token, emailVerifiedAt: DateTime.UtcNow.AddDays(-1));

            var act = () => Command.Execute(token.ToString());

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void Execute_Throws_NotFoundException_When_CodeHasExpired()
        {
            var token = Guid.NewGuid();
            var user = CreateTestUser(token);

            user.CreatedAt = DateTime.UtcNow.AddDays(-2);
            Ctx.SaveChanges();

            var act = () => Command.Execute(token.ToString());

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void Execute_ActivatesAccount_When_CodeIsValid()
        {
            var token = Guid.NewGuid();
            var user = CreateTestUser(token);

            Command.Execute(token.ToString());

            user.EmailVerifiedAt.Should().NotBeNull();
            user.EmailVerificationToken.Should().BeNull();
        }
    }
}
