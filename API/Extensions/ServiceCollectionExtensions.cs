using Application;
using ASPLAB2.API;
using ASPLAB2.API.JWT;
using Implementation;
using Implementation.UseCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwt(this IServiceCollection services, AppSettings settings)
    {
        services.AddTransient<JwtHandler>();
        services.AddTransient<UseCaseHandler>();

        services.AddAuthentication(options =>
        {
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = settings.JwtSettings.Issuer,
                ValidateIssuer = true,
                ValidAudience = "Any",
                ValidateAudience = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(settings.JwtSettings.SecretKey)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            cfg.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var dbContext = context.HttpContext.RequestServices
                        .GetRequiredService<AppDbContext>();
                    var token = context.HttpContext.Request.Headers["Authorization"]
                        .ToString().Split(" ")[1];
                    var tokenObj = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    var tokenId = tokenObj.Claims.FirstOrDefault(x => x.Type == "TokenId")?.Value;
                    var dbToken = dbContext.AuthTokens.FirstOrDefault(x => x.TokenId == tokenId);

                    if (dbToken == null || dbToken.InvalidatedAt.HasValue)
                        context.Fail("Unauthorized");

                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }

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
