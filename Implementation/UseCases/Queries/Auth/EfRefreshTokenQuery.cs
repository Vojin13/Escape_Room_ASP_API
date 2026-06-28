using Application;
using Application.DTO.Auth;
using Application.Exceptions;
using Application.Queries.Auth;
using Microsoft.EntityFrameworkCore;

namespace Implementation.UseCases.Queries.Auth
{
    public class EfRefreshTokenQuery : EfUseCase, IRefreshTokenQuery
    {
        private readonly IJwtHandler _jwtHandler;

        public EfRefreshTokenQuery(AppDbContext context, IJwtHandler handler) : base(context)
        {
            _jwtHandler = handler;
        }

        public string Name => "Refresh User Token";

        public string Id => "refresh-token";

        public JwtTokenResponseDTO Execute(RefreshTokenDTO request)
        {
            var refreshToken = _ctx.AuthTokens
            .Include(t => t.User)
            .ThenInclude(u => u.UserUseCases)
            .FirstOrDefault(t => t.TokenId == request.RefreshTokenId);

            if (refreshToken == null)
                throw new NotFoundException("Token", request.RefreshTokenId);

            if (refreshToken.InvalidatedAt.HasValue)
                throw new UnauthorizedUseCaseException("guest", "Refresh Token");

            if (refreshToken.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedUseCaseException("guest", "Refresh Token");

            if (refreshToken.JwtToken != null)
                refreshToken.JwtToken.InvalidatedAt = DateTime.UtcNow;

            refreshToken.InvalidatedAt = DateTime.UtcNow;

            _ctx.SaveChanges();


            var tokenResponse = _jwtHandler.MakeToken(refreshToken.User);

            return new JwtTokenResponseDTO
            {
                Token = tokenResponse.Token,
                RefreshToken = tokenResponse.RefreshToken,
            };

        }
    }
}
