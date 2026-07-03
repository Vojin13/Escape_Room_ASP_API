using Application.Commands.Bookings;
using Application.DTO.Bookings;
using Application.Exceptions;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using Implementation.UseCases.Validators.Bookings;
using System;
using System.Linq;

namespace Implementation.UseCases.Commands.Bookings
{
    public class EfCreateBookingCommand : EfUseCase, ICreateBookingCommand
    {
        private readonly CreateBookingValidator _validator;

        public EfCreateBookingCommand(AppDbContext context,
                                      CreateBookingValidator validator)
                                      : base(context)
        {
            _validator = validator;
        }

        public string Name => "Create Booking";

        public string Id => "create-booking";

        public void Execute(CreateBookingDTO data)
        {
            _validator.ValidateAndThrow(data);

            var room = _ctx.Rooms.FirstOrDefault(x => x.Id == data.RoomId && x.IsActive);

            if(room == null)
            {
                throw new NotFoundException("Room", data.RoomId);
            }

            var roomTimeslot = _ctx.RoomTimeslots.FirstOrDefault(x => x.TimeslotId == data.TimeslotId && x.RoomId == data.RoomId);

            if(roomTimeslot == null)
            {
                throw new NotFoundException("Timeslot", data.TimeslotId);
            }

            var date = DateTime.SpecifyKind(data.Date.Date, DateTimeKind.Utc);
            var now = DateTime.UtcNow;

            var activeLock = _ctx.TimeslotLocks
                            .FirstOrDefault(x => x.RoomId == data.RoomId 
                            && x.TimeslotId == data.TimeslotId
                            && x.Date == date && x.UserId == data.UserId 
                            && x.ExpiresAt > now);

            if(activeLock == null)
            {
                throw new ConflictException("Your slot hold has expired, please try again.");
            }

            Booking booking = new Booking
            {
                RoomId = data.RoomId,
                TimeslotId = data.TimeslotId,
                BookingDate = date,
                NumberOfPlayers = data.NumberOfPlayers,
                UserId = data.UserId,
                TotalPrice = room.PricePerPerson * data.NumberOfPlayers,
                StatusId = (int)BookingStatus.Pending,
            };

            _ctx.Bookings.Add(booking);
            _ctx.TimeslotLocks.Remove(activeLock);

            _ctx.SaveChanges();
        }
    }
}
