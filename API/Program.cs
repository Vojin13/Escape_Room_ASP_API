// Program.cs
using API.Extensions;
using ASPLAB2.API;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var settings = new AppSettings();
builder.Configuration.Bind(settings);
builder.Services.AddSingleton(settings);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructure(settings.ConnectionStrings.DefaultConnection);
builder.Services.AddImplementation();
builder.Services.AddJwt(settings);
builder.Services.AddApplicationUser();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<JwtMiddleware>();
app.MapControllers();
app.Run();