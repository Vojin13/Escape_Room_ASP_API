using Application.Jobs;
using System.Linq;

namespace Implementation.Jobs
{
    public class CleanupExpiredTokensJob : ICleanupExpiredTokensJob
    {
        private readonly AppDbContext _ctx;

        public CleanupExpiredTokensJob(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Execute()
        {
            var referencedIds = _ctx.AuthTokens
                .Where(x => x.BaseTokenId != null)
                .Select(x => x.BaseTokenId!.Value)
                .Distinct();

            var deletable = _ctx.AuthTokens
                .Where(x => x.ExpiresAt < DateTime.UtcNow && !referencedIds.Contains(x.Id))
                .ToList();

            _ctx.AuthTokens.RemoveRange(deletable);
            _ctx.SaveChanges();
        }
    }
}
