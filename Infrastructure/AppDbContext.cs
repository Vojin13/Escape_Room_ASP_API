using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Implementation
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Timeslot> Timeslots { get; set; }
        public DbSet<RoomTimeslot> RoomTimeslots { get; set; }
        public DbSet<TimeslotLock> TimeslotLocks { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingStatusLookup> BookingStatuses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<UserUseCase> UserUseCases { get; set; }
        public DbSet<AuthToken> AuthTokens { get; set; }
        public DbSet<RoleUseCase> RoleUseCases { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ASP_Escape_Room_API;Username=postgres;Password=komandir1410;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            return base.SaveChanges();
        }
    }
}
