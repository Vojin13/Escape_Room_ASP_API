using Application.Commands;
using Application.DTO;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Implementation.UseCases.Commands
{
    public class EfSeedCommand : EfUseCase, ISeedCommand
    {
        public string Name => "Seed Database";

        public string Id => "seed";

        public EfSeedCommand(AppDbContext ctx) : base(ctx) { }

        public void Execute(NoData data)
        {
            SeedRolesAndUseCases();
            SeedTimeslots();
            SeedUsers();
            SeedRooms();
            SeedBookings();
            SeedReviews();
        }

        private void SeedRolesAndUseCases()
        {
            if (_ctx.Roles.Any()) return;

            var userRole = new Role { Name = "User", Slug = "user" };
            var adminRole = new Role { Name = "Admin", Slug = "admin" };

            _ctx.Roles.AddRange(userRole, adminRole);
            _ctx.SaveChanges();

            var userUseCases = new List<string>
        {
            "get-rooms", "get-room", "get-room-availability",
            "lock-timeslot", "create-booking", "cancel-booking",
            "create-review", "get-my-bookings", "get-booking",
            "get-room-reviews", "get-my-profile", "update-my-profile",
            "logout", "register-user", "login"
        };

            var adminUseCases = userUseCases.Concat(new List<string>
        {
            "create-room", "update-room", "toggle-room-active",
            "create-blockade", "delete-blockade", "get-blockades",
            "delete-review", "get-users", "update-user-role",
            "get-all-bookings", "seed"
        }).ToList();

            _ctx.RoleUseCases.AddRange(userUseCases.Select(uc => new RoleUseCase
            {
                RoleId = userRole.Id,
                UseCaseId = uc
            }));

            _ctx.RoleUseCases.AddRange(adminUseCases.Select(uc => new RoleUseCase
            {
                RoleId = adminRole.Id,
                UseCaseId = uc
            }));

            _ctx.SaveChanges();
        }

        private void SeedTimeslots()
        {
            if (_ctx.Timeslots.Any()) return;

            _ctx.Timeslots.AddRange(
                new Timeslot { StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 30) },
                new Timeslot { StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(13, 30) },
                new Timeslot { StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30) },
                new Timeslot { StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(17, 30) },
                new Timeslot { StartTime = new TimeOnly(18, 0), EndTime = new TimeOnly(19, 30) },
                new Timeslot { StartTime = new TimeOnly(20, 0), EndTime = new TimeOnly(21, 30) }
            );

            _ctx.SaveChanges();
        }

        private void SeedUsers()
        {
            if (_ctx.Users.Any()) return;

            var adminRole = _ctx.Roles.First(r => r.Slug == "admin");
            var userRole = _ctx.Roles.First(r => r.Slug == "user");

            // Admin
            var admin = new User
            {
                Username = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@cipherescape.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                RoleId = adminRole.Id
            };

            _ctx.Users.Add(admin);
            _ctx.SaveChanges();
            AssignRoleUseCases(admin.Id, adminRole.Id);

            // Fake users
            var faker = new Faker("en");

            for (int i = 0; i < 20; i++)
            {
                var firstName = faker.Name.FirstName();
                var lastName = faker.Name.LastName();

                var user = new User
                {
                    Username = faker.Internet.UserName(firstName, lastName),
                    FirstName = firstName,
                    LastName = lastName,
                    Email = faker.Internet.Email(firstName, lastName),
                    Password = BCrypt.Net.BCrypt.HashPassword("User1234!"),
                    RoleId = userRole.Id,
                    EmailVerifiedAt = faker.Random.Bool(0.7f) ? DateTime.UtcNow.AddDays(-faker.Random.Int(1, 30)) : null
                };

                _ctx.Users.Add(user);
                _ctx.SaveChanges();
                AssignRoleUseCases(user.Id, userRole.Id);
            }
        }

        private void SeedRooms()
        {
            if (_ctx.Rooms.Any()) return;

            var faker = new Faker("en");
            var difficulties = _ctx.Difficulties.ToList();
            var timeslots = _ctx.Timeslots.ToList();

            var roomNames = new[]
            {
            "The Haunted Mansion", "Prison Break", "Lost in Space",
            "The Da Vinci Code", "Zombie Apocalypse", "The Pharaoh's Tomb",
            "Murder Mystery", "The Bunker", "Cyber Heist", "Sherlock's Study",
            "The Lost Temple", "Atlantis Rising", "The Time Machine",
            "Witch's Lair", "The Submarine"
        };

            var descriptions = new[]
            {
            "A chilling adventure through a haunted Victorian mansion. Solve the mystery before the ghost finds you.",
            "You've been wrongfully imprisoned. Use your wits to escape before the guards return.",
            "Stranded on a space station with limited oxygen. Fix the systems before it's too late.",
            "Crack the codes hidden by a secret society and unveil the greatest mystery of all time.",
            "The dead have risen. Find the cure before you become one of them.",
            "Explore the ancient tomb of a pharaoh and escape with the treasure before the traps close in.",
            "A murder has been committed. You are the prime suspect. Prove your innocence.",
            "Hidden deep underground, the bunker holds secrets that were never meant to be found.",
            "Infiltrate a high-tech corporation and steal the data without triggering the alarms.",
            "Help the world's greatest detective solve an impossible case in his iconic study.",
            "Discover the secrets of an ancient civilization before the jungle reclaims the temple.",
            "Dive deep into the mysteries of the lost city beneath the ocean.",
            "Travel through time to prevent a catastrophic event from erasing history.",
            "Break the witch's curse before midnight or be trapped in her lair forever.",
            "Navigate the depths of a sinking submarine and find your way to the surface."
        };

            for (int i = 0; i < roomNames.Length; i++)
            {
                var minPlayers = faker.Random.Int(2, 4);
                var difficulty = faker.Random.ArrayElement(difficulties.ToArray());

                var room = new Room
                {
                    Title = roomNames[i],
                    Description = descriptions[i],
                    MinimumPlayers = (short)minPlayers,
                    MaximumPlayers = (short)faker.Random.Int(minPlayers + 1, 8),
                    DurationInMinutes = (short)faker.Random.ArrayElement(new[] { 60, 75, 90 }),
                    PricePerPerson = Math.Round(faker.Random.Decimal(8, 35), 2),
                    IsActive = faker.Random.Bool(0.9f),
                    DifficultyId = difficulty.Id
                };

                _ctx.Rooms.Add(room);
                _ctx.SaveChanges();

                // Dodaj timeslotove sobi
                _ctx.RoomTimeslots.AddRange(timeslots.Select(t => new RoomTimeslot
                {
                    RoomId = room.Id,
                    TimeslotId = t.Id
                }));

                _ctx.SaveChanges();
            }
        }

        private void SeedBookings()
        {
            if (_ctx.Bookings.Any()) return;

            var faker = new Faker("en");
            var users = _ctx.Users.Where(u => u.Role.Slug == "user").ToList();
            var rooms = _ctx.Rooms.Where(r => r.IsActive).ToList();
            var timeslots = _ctx.Timeslots.ToList();
            var statuses = _ctx.BookingStatuses.ToList();

            for (int i = 0; i < 50; i++)
            {
                var user = faker.Random.ArrayElement(users.ToArray());
                var room = faker.Random.ArrayElement(rooms.ToArray());
                var timeslot = faker.Random.ArrayElement(timeslots.ToArray());
                var status = faker.Random.ArrayElement(statuses.ToArray());
                var numberOfPlayers = faker.Random.Int(room.MinimumPlayers, room.MaximumPlayers);

                var booking = new Booking
                {
                    UserId = user.Id,
                    RoomId = room.Id,
                    TimeslotId = timeslot.Id,
                    StatusId = status.Id,
                    BookingDate = faker.Date.Between(DateTime.UtcNow.AddMonths(-3), DateTime.UtcNow.AddMonths(2)),
                    NumberOfPlayers = numberOfPlayers,
                    TotalPrice = room.PricePerPerson * numberOfPlayers
                };

                _ctx.Bookings.Add(booking);
            }

            _ctx.SaveChanges();
        }

        private void SeedReviews()
        {
            if (_ctx.Reviews.Any()) return;

            var faker = new Faker("en");

            var completedBookings = _ctx.Bookings
                .Where(b => b.StatusId == (int)BookingStatus.Completed)
                .Include(b => b.User)
                .Include(b => b.Room)
                .ToList();

            var reviewed = new HashSet<(int userId, int roomId)>();

            foreach (var booking in completedBookings)
            {
                var key = (booking.UserId, booking.RoomId);
                if (reviewed.Contains(key)) continue;

                if (!faker.Random.Bool(0.7f)) continue;

                _ctx.Reviews.Add(new Review
                {
                    UserId = booking.UserId,
                    RoomId = booking.RoomId,
                    Rating = (byte)faker.Random.Int(1, 5),
                    Comment = faker.Random.Bool(0.8f) ? faker.Lorem.Sentence() : null
                });

                reviewed.Add(key);
            }

            _ctx.SaveChanges();
        }
    }
}

