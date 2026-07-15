using Application;
using ASPLAB2.API.JWT;
using Implementation;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace API.Extensions;

public static class ApplicationUserServiceExtensions
{
    public static IServiceCollection AddApplicationUser(this IServiceCollection services)
    {
        services.AddTransient<IApplicationUser>(container =>
        {
            var accessor = container.GetService<IHttpContextAccessor>();

            if (accessor.HttpContext == null)
                return new UnauthorizedUser();

            if (!accessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
                return new UnauthorizedUser();

            var header = accessor.HttpContext.Request.Headers.Authorization;
            var headerParts = header.ToString().Split(" ");

            if (headerParts.Count() != 2 || headerParts[0] != "Bearer")
                return new UnauthorizedUser();

            try
            {
                var token = headerParts[1];
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var useCaseIds = jwtToken.Claims
                    .FirstOrDefault(x => x.Type == "UseCaseIds")?.Value;

                return new JwtUser
                {
                    Id = int.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value),
                    Username = jwtToken.Claims.First(x => x.Type == "Username").Value,
                    Email = jwtToken.Claims.First(x => x.Type == "Email").Value,
                    AllowedUseCases = useCaseIds != null
                        ? JsonConvert.DeserializeObject<List<string>>(useCaseIds)
                        : new List<string>()
                };
            }
            catch
            {
                return new UnauthorizedUser();
            }
        });

        return services;
    }
}
