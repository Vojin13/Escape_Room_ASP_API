using Application.DTO.Auth;
using Application.Exceptions;
using Domain.Entities;
using FluentAssertions;
using Implementation.UseCases.Queries.Auth;
using Tests.Common;
using Xunit;

namespace Tests.UseCases.Auth
{
    public class RefreshTokenTests : TestBase
    {
        /*
            Svaki test se sastoji iz:
            1. Postavljanje inicijalnog (poznatog) stanja
            2. Izvrsavanja testa (algoritam)
            3. Verifikacije rezultata
        */

        private EfRefreshTokenQuery Query => new EfRefreshTokenQuery(Ctx, JwtHandler);

        private User CreateTestUser()
        {
            var role = new Role { Name = "User", Slug = "user" };
            Ctx.Roles.Add(role);
            Ctx.SaveChanges();

            var user = new User
            {
                Username = $"jwttestuser_{Guid.NewGuid():N}",
                FirstName = "Jwt",
                LastName = "Test",
                Email = $"jwttest_{Guid.NewGuid():N}@example.com",
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
            var request = new RefreshTokenDTO { RefreshTokenId = Guid.NewGuid().ToString() };

            var act = () => Query.Execute(request);

            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void Execute_Throws_UnauthorizedUseCaseException_When_TokenHasExpired()
        {
            var user = CreateTestUser();
            var tokenId = Guid.NewGuid().ToString();

            var authToken = new AuthToken
            {
                TokenId = tokenId,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                ExpiresAt = DateTime.UtcNow.AddSeconds(-3)
            };

            Ctx.AuthTokens.Add(authToken);
            Ctx.SaveChanges();

            var request = new RefreshTokenDTO { RefreshTokenId = tokenId };

            var act = () => Query.Execute(request);

            act.Should().Throw<UnauthorizedUseCaseException>();
        }

        [Fact]
        public void Execute_Throws_UnauthorizedUseCaseException_When_TokenIsInvalidated()
        {
            var user = CreateTestUser();
            var tokenId = Guid.NewGuid().ToString();

            var authToken = new AuthToken
            {
                TokenId = tokenId,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                ExpiresAt = DateTime.UtcNow.AddSeconds(30),
                InvalidatedAt = DateTime.UtcNow.AddSeconds(-10)
            };

            Ctx.AuthTokens.Add(authToken);
            Ctx.SaveChanges();

            var request = new RefreshTokenDTO { RefreshTokenId = tokenId };

            var act = () => Query.Execute(request);

            act.Should().Throw<UnauthorizedUseCaseException>();
        }

        [Fact]
        public void Execute_Returns_NewTokenAndRefreshToken_When_RefreshTokenIsValid()
        {
            var user = CreateTestUser();
            var tokenId = Guid.NewGuid().ToString();

            var authJwtToken = new AuthToken
            {
                TokenId = Guid.NewGuid().ToString(),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                ExpiresAt = DateTime.UtcNow.AddSeconds(5)
            };

            var authRefreshToken = new AuthToken
            {
                TokenId = tokenId,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                ExpiresAt = DateTime.UtcNow.AddSeconds(30),
                JwtToken = authJwtToken
            };

            Ctx.AuthTokens.Add(authRefreshToken);
            Ctx.SaveChanges();

            var request = new RefreshTokenDTO { RefreshTokenId = tokenId };

            var result = Query.Execute(request);

            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
        }
    }
}
