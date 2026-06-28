using Application.Commands;
using Application.Commands.Auth;
using Implementation.Mappings;
using Implementation.UseCases.Commands;
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

        // Queries

        return services;
    }
}