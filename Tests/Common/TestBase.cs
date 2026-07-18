using Application;
using ASPLAB2.API;
using ASPLAB2.API.JWT;
using Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Tests.Common
{
    public abstract class TestBase : IDisposable
    {
        protected readonly AppDbContext Ctx;
        protected readonly IJwtHandler JwtHandler;
        protected readonly TestApplicationUser User;
        protected readonly NoOpCacheService Cache;

        private readonly IDbContextTransaction _transaction;

        protected TestBase()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();

            var settings = new AppSettings();
            configuration.Bind(settings);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(settings.ConnectionStrings.DefaultConnection)
                .Options;

            Ctx = new AppDbContext(options);
            _transaction = Ctx.Database.BeginTransaction();

            JwtHandler = new JwtHandler(settings, Ctx);
            User = new TestApplicationUser();
            Cache = new NoOpCacheService();
        }

        public void Dispose()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            Ctx.Dispose();
        }
    }
}
