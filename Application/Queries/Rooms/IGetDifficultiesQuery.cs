using Application.DTO;

namespace Application.Queries.Rooms
{
    public interface IGetDifficultiesQuery : IQuery<NoData, IEnumerable<LookupDTO>>
    {
    }
}
