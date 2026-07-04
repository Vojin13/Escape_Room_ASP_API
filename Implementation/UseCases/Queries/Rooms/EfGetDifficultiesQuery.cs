using Application.DTO;
using Application.Queries.Rooms;

namespace Implementation.UseCases.Queries.Rooms
{
    public class EfGetDifficultiesQuery : EfUseCase, IGetDifficultiesQuery
    {
        public EfGetDifficultiesQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Get Difficulties";

        public string Id => "get-difficulties";

        public IEnumerable<LookupDTO> Execute(NoData request)
        {
            return _ctx.Difficulties
                .Select(x => new LookupDTO { Id = x.Id, Name = x.Name })
                .ToList();
        }
    }
}
