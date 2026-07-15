using Application.Jobs;
using Hangfire;

namespace API.Extensions;

public static class ScheduledJobsApplicationExtensions
{
    public static WebApplication UseScheduledJobs(this WebApplication app)
    {
        app.UseHangfireDashboard("/hangfire");

        RecurringJob.AddOrUpdate<ICleanupExpiredTokensJob>(
            "cleanup-expired-tokens", job => job.Execute(), Cron.Daily());
        RecurringJob.AddOrUpdate<ICompleteExpiredBookingsJob>(
            "complete-expired-bookings", job => job.Execute(), Cron.Daily());
        RecurringJob.AddOrUpdate<ICleanupExpiredTimeslotLocksJob>(
            "cleanup-expired-timeslot-locks", job => job.Execute(), Cron.Hourly());

        return app;
    }
}
