using API.Extensions;
using Application;
using Application.Emails;
using ASPLAB2.API;
using ASPLAB2.API.JWT;
using Implementation.Emails;
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
builder.Services.AddTransient<IJwtHandler, JwtHandler>();
builder.Services.AddTransient<IEmailSender>(_ => new SmtpEmailSender(
    settings.MailSettings.FromEmail,
    settings.MailSettings.AppPassword));
builder.Services.AddApplicationUser();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAngular");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<JwtMiddleware>();
app.MapControllers();
app.Run();