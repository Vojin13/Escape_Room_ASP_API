using Application.DTO;
using Application.DTO.Bookings;

namespace Application.Queries.Bookings
{
    public interface IGetMyCartQuery : IQuery<NoData, MyCartDTO>
    {
    }
}
