using Application.Commands;
using Application.Commands.Auth;
using Application.Commands.Rooms;
using Application.Queries.Auth;
using Application.Queries.Rooms;
using Application.Queries.Rooms.Admin;
using Application.Queries.Users;
using Implementation.UseCases.Queries.Rooms.Admin;
using Implementation.UseCases.Queries.Rooms;
using Implementation.UseCases.Queries.Users;
using Implementation.Mappings;
using Implementation.UseCases.Commands;
using Implementation.UseCases.Commands.Auth;
using Implementation.UseCases.Commands.Rooms;
using Implementation.UseCases.Queries.Auth;
using Implementation.UseCases.Validators.Auth;
using Implementation.UseCases.Validators.Rooms;
using Implementation.UseCases.Validators.Users;
using Microsoft.Extensions.DependencyInjection;
using Application.Queries.Users.Admin;
using Implementation.UseCases.Queries.Users.Admin;
using Application.Commands.Users;
using Implementation.UseCases.Commands.Users.Admin;
using Application.Queries.Timeslots.Admin;
using Implementation.UseCases.Queries.Timeslots.Admin;
using Application.Commands.Timeslots;
using Implementation.UseCases.Commands.Timeslots.Admin;
using Implementation.UseCases.Validators.Timeslots;
using Implementation.Emails;
using Application.Commands.Bookings;
using Implementation.UseCases.Commands.Bookings;
using Application.Queries.Bookings;
using Implementation.UseCases.Queries.Bookings;
using Implementation.UseCases.Validators.Bookings;

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
        services.AddTransient<CreateUserValidator>();
        services.AddTransient<UpdateUserValidator>();
        services.AddTransient<CreateTimeslotValidator>();
        services.AddTransient<UpdateTimeslotValidator>();
        services.AddTransient<EmailTemplateComposer>();
        services.AddTransient<CreateBookingValidator>();

        // Commands
        services.AddTransient<IRegisterUserCommand, EfRegisterUserCommand>();
        services.AddTransient<ISeedCommand, EfSeedCommand>();
        services.AddTransient<ILogoutUserCommand, EfLogoutUserCommand>();
        services.AddTransient<IDeleteRoomCommand, EfDeleteRoomCommand>();
        services.AddTransient<ICreateRoomCommand, EfCreateRoomCommand>();
        services.AddTransient<IUpdateRoomCommand, EfUpdateRoomCommand>();
        services.AddTransient<IDeleteUserCommand, EfDeleteUserCommand>();
        services.AddTransient<ICreateUserCommand, EfCreateUserCommand>();
        services.AddTransient<IUpdateUserCommand, EfUpdateUserCommand>();
        services.AddTransient<ICreateTimeslotCommand, EfCreateTimeslotCommand>();
        services.AddTransient<IUpdateTimeslotCommand, EfUpdateTimeslotCommand>();
        services.AddTransient<IDeleteTimeslotCommand, EfDeleteTimeslotCommand>();
        services.AddTransient<IToggleRoomActiveCommand, EfToggleRoomActiveCommand>();
        services.AddTransient<IActivateAccountCommand, EfActivateAccountCommand>();
        services.AddTransient<ICreateBookingCommand, EfCreateBookingCommand>();

        // Queries
        services.AddTransient<ILoginUserQuery, EfLoginUserQuery>();
        services.AddTransient<IRefreshTokenQuery, EfRefreshTokenQuery>();
        services.AddTransient<IGetRoomsQuery, EfGetRoomsQuery>();
        services.AddTransient<IGetRoomQuery, EfGetRoomQuery>();
        services.AddTransient<IGetRoomAvailabilityQuery, EfGetRoomAvailabilityQuery>();
        services.AddTransient<IAdminGetRoomsQuery, EfAdminGetRoomsQuery>();
        services.AddTransient<IAdminGetRoomQuery, EfAdminGetRoomQuery>();
        services.AddTransient<IGetMyProfileQuery, EfGetMyProfileQuery>();
        services.AddTransient<IAdminGetUsersQuery, EfAdminGetUsersQuery>();
        services.AddTransient<IAdminGetUserQuery, EfAdminGetUserQuery>();
        services.AddTransient<IAdminGetTimeslotsQuery, EfAdminGetTimeslotsQuery>();
        services.AddTransient<IAdminGetTimeslotQuery, EfAdminGetTimeslotQuery>();
        services.AddTransient<ILockTimeslotQuery, EfLockTimeslotQuery>();

        return services;
    }
}