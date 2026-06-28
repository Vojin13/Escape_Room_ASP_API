using Application;
using Application.DTO.Auth;
using Application.Exceptions;
using Application.Queries.Auth;
using Microsoft.EntityFrameworkCore;

namespace Implementation.UseCases.Queries.Auth
{
    public class EfLoginUserQuery : EfUseCase, ILoginUserQuery
    {
        private readonly IJwtHandler _jwtHandler;

        public string Name => "Login User";

        public string Id => "login";

        public EfLoginUserQuery(AppDbContext ctx, IJwtHandler handler) : base(ctx)
        {
            _jwtHandler = handler;
        }

        public JwtTokenResponseDTO Execute(LoginDTO request)
        {
            var user = _ctx.Users
                .Include(x => x.UserUseCases)
                .FirstOrDefault(x => x.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                throw new InvalidCredentialsException();
            }

            var tokenResponse = _jwtHandler.MakeToken(user);

            return new JwtTokenResponseDTO
            {
                Token = tokenResponse.Token,
                RefreshToken = tokenResponse.RefreshToken,
            };
        }
    }
}
