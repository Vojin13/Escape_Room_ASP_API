using Application.DTO.Auth;
using Application.Exceptions;
using Domain.Entities;
using FluentAssertions;
using Implementation.UseCases.Commands.Auth;
using Tests.Common;
using Xunit;

namespace Tests.UseCases.Auth
{
    public class LogoutTests : TestBase
    {
        private EfLogoutUserCommand Command => new EfLogoutUserCommand(Ctx);

        private User CreateTestUser()
        {
            var role = new Role { Name = "User", Slug = "user" };
            Ctx.Roles.Add(role);
            Ctx.SaveChanges();

            var user = new User
            {
                Username = $"logoutuser_{Guid.NewGuid():N}",
                FirstName = "Logout",
                LastName = "Test",
                Email = $"logout_{Guid.NewGuid():N}@example.com",
                Password = "hashed",
                RoleId = role.Id
            };
            Ctx.Users.Add(user);
            Ctx.SaveChanges();

            return user;
        }

        [Fact]
        public void Execute_Throws_NotFoundException_When_RefreshTokenDoesNotExist()
        {
            var request = new LogoutDTO { RefreshTokenId = Guid.NewGuid().ToString() };

            var act = () => Command.Execute(request);

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void Execute_Invalidates_RefreshTokenAndLinkedJwtToken()
        {
            var user = CreateTestUser();
            var tokenId = Guid.NewGuid().ToString();

            var jwtToken = new AuthToken
            {
                TokenId = Guid.NewGuid().ToString(),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

            var refreshToken = new AuthToken
            {
                TokenId = tokenId,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                JwtToken = jwtToken
            };

            Ctx.AuthTokens.Add(refreshToken);
            Ctx.SaveChanges();

            var request = new LogoutDTO { RefreshTokenId = tokenId };

            Command.Execute(request);

            refreshToken.InvalidatedAt.Should().NotBeNull();
            jwtToken.InvalidatedAt.Should().NotBeNull();
        }

        [Fact]
        public void Execute_Invalidates_RefreshTokenOnly_When_NoLinkedJwtToken()
        {
            var user = CreateTestUser();
            var tokenId = Guid.NewGuid().ToString();

            var refreshToken = new AuthToken
            {
                TokenId = tokenId,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            Ctx.AuthTokens.Add(refreshToken);
            Ctx.SaveChanges();

            var request = new LogoutDTO { RefreshTokenId = tokenId };

            var act = () => Command.Execute(request);

            act.Should().NotThrow();
            refreshToken.InvalidatedAt.Should().NotBeNull();
        }
    }
}
