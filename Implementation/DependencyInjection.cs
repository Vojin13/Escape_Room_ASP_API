using Application.Commands;
using Application.Commands.Auth;
using Application.Commands.Rooms;
using Application.Queries.Auth;
using Application.Queries.Rooms;
using Implementation.Mappings;
using Implementation.UseCases.Commands;
using Implementation.UseCases.Commands.Auth;
using Implementation.UseCases.Commands.Rooms;
using Implementation.UseCases.Queries.Auth;
using Implementation.UseCases.Queries.Rooms;
using Implementation.UseCases.Validators.Auth;
using Implementation.UseCases.Validators.Rooms;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddImplementation(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(cfg => { }, typeof(UserProfile),
                                           typeof(RoomProfile));

        // Validators
        services.AddTransient<RegisterUserValidator>();
        services.AddTransient<CreateRoomValidator>();
        services.AddTransient<UpdateRoomValidator>();

        // Commands
        services.AddTransient<IRegisterUserCommand, EfRegisterUserCommand>();
        services.AddTransient<ISeedCommand, EfSeedCommand>();
        services.AddTransient<ILogoutUserCommand, EfLogoutUserCommand>();
        services.AddTransient<IDeleteRoomCommand, EfDeleteRoomCommand>();
        services.AddTransient<ICreateRoomCommand, EfCreateRoomCommand>();
        services.AddTransient<IUpdateRoomCommand, EfUpdateRoomCommand>();

        // Queries
        services.AddTransient<ILoginUserQuery, EfLoginUserQuery>();
        services.AddTransient<IRefreshTokenQuery, EfRefreshTokenQuery>();
        services.AddTransient<IGetRoomsQuery, EfGetRoomsQuery>();
        services.AddTransient<IGetRoomQuery, EfGetRoomQuery>();
        services.AddTransient<IGetRoomAvailabilityQuery, EfGetRoomAvailabilityQuery>();

        return services;
    }
}