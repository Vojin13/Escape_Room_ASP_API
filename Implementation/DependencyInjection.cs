using Application.Commands;
using Application.Commands.Auth;
using Application.Queries.Auth;
using Implementation.Mappings;
using Implementation.UseCases.Commands;
using Implementation.UseCases.Commands.Auth;
using Implementation.UseCases.Queries.Auth;
using Implementation.UseCases.Validators.Auth;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddImplementation(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(cfg => { }, typeof(UserProfile));

        // Validators
        services.AddTransient<RegisterUserValidator>();

        // Commands
        services.AddTransient<IRegisterUserCommand, EfRegisterUserCommand>();
        services.AddTransient<ISeedCommand, EfSeedCommand>();
        services.AddTransient<ILogoutUserCommand, EfLogoutUserCommand>();
        services.AddTransient<IRefreshTokenQuery, EfRefreshTokenQuery>();

        // Queries
        services.AddTransient<ILoginUserQuery, EfLoginUserQuery>();

        return services;
    }
}