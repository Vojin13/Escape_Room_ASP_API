using Application.DTO.Auth;
using Application.Exceptions;
using Domain.Entities;
using FluentAssertions;
using Implementation.UseCases.Queries.Auth;
using Tests.Common;
using Xunit;

namespace Tests.UseCases.Auth
{
    public class LoginTests : TestBase
    {
        private EfLoginUserQuery Query => new EfLoginUserQuery(Ctx, JwtHandler);

        private User CreateTestUser(string password = "Password123!", bool isDeleted = false)
        {
            var role = new Role { Name = "User", Slug = "user" };
            Ctx.Roles.Add(role);
            Ctx.SaveChanges();

            var user = new User
            {
                Username = $"loginuser_{Guid.NewGuid():N}",
                FirstName = "Login",
                LastName = "Test",
                Email = $"login_{Guid.NewGuid():N}@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                RoleId = role.Id,
                IsDeleted = isDeleted
            };
            Ctx.Users.Add(user);
            Ctx.SaveChanges();

            return user;
        }

        [Fact]
        public void Execute_Throws_InvalidCredentialsException_When_UserDoesNotExist()
        {
            var request = new LoginDTO { Email = "nonexistent@example.com", Password = "Password123!" };

            var act = () => Query.Execute(request);

            act.Should().Throw<InvalidCredentialsException>();
        }

        [Fact]
        public void Execute_Throws_InvalidCredentialsException_When_UserIsDeleted()
        {
            var user = CreateTestUser(isDeleted: true);

            var request = new LoginDTO { Email = user.Email, Password = "Password123!" };

            var act = () => Query.Execute(request);

            act.Should().Throw<InvalidCredentialsException>();
        }

        [Fact]
        public void Execute_Throws_InvalidCredentialsException_When_PasswordIsWrong()
        {
            var user = CreateTestUser();

            var request = new LoginDTO { Email = user.Email, Password = "WrongPassword123!" };

            var act = () => Query.Execute(request);

            act.Should().Throw<InvalidCredentialsException>();
        }

        [Fact]
        public void Execute_Returns_JwtTokenResponseDTO_When_CredentialsAreValid()
        {
            var user = CreateTestUser("Password123!");

            var request = new LoginDTO { Email = user.Email, Password = "Password123!" };

            var result = Query.Execute(request);

            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
        }
    }
}
