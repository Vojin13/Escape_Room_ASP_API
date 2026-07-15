using Application;
using ASPLAB2.API;
using ASPLAB2.API.JWT;
using Implementation;
using Implementation.UseCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace API.Extensions;

public static class JwtServiceExtensions
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
}
