using Application.DTO.Bookings;

namespace Application.Commands.Bookings
{
    public interface ICreateBookingCommand : ICommand<CreateBookingDTO>
    {
    }
}
