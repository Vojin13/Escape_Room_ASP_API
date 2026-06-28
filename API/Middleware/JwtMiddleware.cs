using ASPLAB2.API.JWT;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var header = context.Request.Headers["Authorization"].ToString();

        if (header.StartsWith("Bearer "))
        {
            var token = header.Replace("Bearer ", "");

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var parsed = handler.ReadJwtToken(token);

                var useCaseIds = parsed.Claims
                    .FirstOrDefault(c => c.Type == "UseCaseIds")?.Value;

                context.Items["ApplicationUser"] = new JwtUser
                {
                    Id = int.Parse(parsed.Claims.First(c => c.Type == "Id").Value),
                    Email = parsed.Claims.First(c => c.Type == "Email").Value,
                    Username = parsed.Claims.First(c => c.Type == "Username").Value,
                    AllowedUseCases = useCaseIds != null
                        ? JsonConvert.DeserializeObject<List<string>>(useCaseIds)
                        : new List<string>()
                };
            }
            catch { }
        }

        await _next(context);
    }
}