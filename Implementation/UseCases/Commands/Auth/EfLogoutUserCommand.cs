using Application.Commands.Auth;
using Application.DTO.Auth;
using Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Implementation.UseCases.Commands.Auth
{
    public class EfLogoutUserCommand : EfUseCase, ILogoutUserCommand
    {
        public string Name => "Logout user";

        public string Id => "logout";

        public EfLogoutUserCommand(AppDbContext ctx) : base(ctx) { }

        public void Execute(LogoutDTO request)
        {
            var refreshToken = _ctx.AuthTokens
            .Include(t => t.JwtToken)
            .FirstOrDefault(t => t.TokenId == request.RefreshTokenId);

            if (refreshToken == null)
            {
                throw new NotFoundException("Token", request.RefreshTokenId);
            }

            refreshToken.InvalidatedAt = DateTime.UtcNow;

            if (refreshToken.JwtToken != null)
            {
                refreshToken.JwtToken.InvalidatedAt = DateTime.UtcNow;
            }

            _ctx.SaveChanges();
        }
    }
}
