using Application.Commands;
using Application.Commands.Auth;
using Application.Queries.Auth;
using Application.Queries.Rooms;
using Implementation.Mappings;
using Implementation.UseCases.Commands;
using Implementation.UseCases.Commands.Auth;
using Implementation.UseCases.Queries.Auth;
using Implementation.UseCases.Queries.Rooms;
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
        services.AddTransient<IGetRoomsQuery, EfGetRoomsQuery>();
        services.AddTransient<IGetRoomQuery, EfGetRoomQuery>();
        services.AddTransient<IGetRoomAvailabilityQuery, EfGetRoomAvailabilityQuery>();

        return services;
    }
}