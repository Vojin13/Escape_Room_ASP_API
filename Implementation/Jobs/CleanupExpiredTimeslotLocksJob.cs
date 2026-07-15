using Application.Jobs;
using System.Linq;

namespace Implementation.Jobs
{
    public class CleanupExpiredTimeslotLocksJob : ICleanupExpiredTimeslotLocksJob
    {
        private readonly AppDbContext _ctx;

        public CleanupExpiredTimeslotLocksJob(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Execute()
        {
            var expiredLocks = _ctx.TimeslotLocks
                .Where(x => x.ExpiresAt < DateTime.UtcNow)
                .ToList();

            _ctx.TimeslotLocks.RemoveRange(expiredLocks);
            _ctx.SaveChanges();
        }
    }
}
