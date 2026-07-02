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
                "logout", "register-user", "login", "refresh-token"
            };

            var adminUseCases = userUseCases.Concat(new List<string>
            {
                "create-room", "update-room", "toggle-room-active",
                "create-blockade", "delete-blockade", "get-blockades",
                "delete-review", "get-users", "update-user-role",
                "get-all-bookings", "seed", "delete-room", "admin-get-rooms", "admin-get-room",
                "delete-room", "admin-get-users", "admin-get-user", "update-user", "delete-user"
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
                new Timeslot { StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(14, 0) },
                new Timeslot { StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(16, 0) },
                new Timeslot { StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(18, 0) },
                new Timeslot { StartTime = new TimeOnly(18, 0), EndTime = new TimeOnly(20, 0) },
                new Timeslot { StartTime = new TimeOnly(20, 0), EndTime = new TimeOnly(22, 0) }
            );

            _ctx.SaveChanges();
        }

        private void SeedUsers()
        {
            if (_ctx.Users.Any()) return;

            var adminRole = _ctx.Roles.First(r => r.Slug == "admin");
            var userRole = _ctx.Roles.First(r => r.Slug == "user");

            var admin = new User
            {
                Username = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@cipherescape.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                RoleId = adminRole.Id
            };

            var admin2 = new User
            {
                Username = "vojin",
                FirstName = "Vojin",
                LastName = "Konatarevic",
                Email = "konatarevicv@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("test123"),
                RoleId = adminRole.Id
            };

            _ctx.Users.Add(admin);
            _ctx.Users.Add(admin2);
            _ctx.SaveChanges();
            AssignRoleUseCases(admin.Id, adminRole.Id);

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

            var availableImages = new[]
            {
                "/images/rooms/18/93df7cb0-26ad-4bb8-9f9a-0161d4b7cf2d.png",
                "/images/rooms/18/e1ca1636-715d-442e-a10e-51ff3678275a.jpg",
                "/images/rooms/19/4a8b8797-29c9-4cc8-b072-56ccd3749c55.png",
                "/images/rooms/19/e0feecd1-68ec-47c6-bfa2-d146ce286c41.jpg"
            };

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

            var durations = new short[] { 45, 60, 90 };
            var prices = Enumerable.Range(10, 51)
                .SelectMany(i => new[] { i * 1.0m, i + 0.50m })
                .Where(p => p >= 10 && p <= 35)
                .ToArray();

            for (int i = 0; i < roomNames.Length; i++)
            {
                var minPlayers = (short)faker.Random.Int(2, 4);
                var difficulty = faker.Random.ArrayElement(difficulties.ToArray());

                var room = new Room
                {
                    Title = roomNames[i],
                    Description = descriptions[i],
                    MinimumPlayers = minPlayers,
                    MaximumPlayers = (short)faker.Random.Int(minPlayers + 1, 8),
                    DurationInMinutes = faker.Random.ArrayElement(durations),
                    PricePerPerson = faker.Random.ArrayElement(prices),
                    IsActive = faker.Random.Bool(0.9f),
                    DifficultyId = difficulty.Id
                };

                _ctx.Rooms.Add(room);
                _ctx.SaveChanges();

                _ctx.RoomTimeslots.AddRange(timeslots.Select(t => new RoomTimeslot
                {
                    RoomId = room.Id,
                    TimeslotId = t.Id
                }));

                var imageCount = faker.Random.Int(3, 4);
                for (int j = 0; j < imageCount; j++)
                {
                    var imgPath = availableImages[(i * imageCount + j) % availableImages.Length];
                    var ext = Path.GetExtension(imgPath).TrimStart('.');

                    _ctx.RoomImages.Add(new RoomImage
                    {
                        RoomId = room.Id,
                        Path = imgPath,
                        MimeType = $"image/{ext}",
                        Size = 0,
                        IsPrimary = j == 0,
                        SortOrder = j
                    });
                }

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

            for (int i = 0; i < 80; i++)
            {
                var user = faker.Random.ArrayElement(users.ToArray());
                var room = faker.Random.ArrayElement(rooms.ToArray());
                var timeslot = faker.Random.ArrayElement(timeslots.ToArray());
                var status = faker.Random.ArrayElement(statuses.ToArray());
                var numberOfPlayers = faker.Random.Int(room.MinimumPlayers, room.MaximumPlayers);

                _ctx.Bookings.Add(new Booking
                {
                    UserId = user.Id,
                    RoomId = room.Id,
                    TimeslotId = timeslot.Id,
                    StatusId = status.Id,
                    BookingDate = faker.Date.Between(DateTime.UtcNow.AddMonths(-3), DateTime.UtcNow.AddMonths(2)),
                    NumberOfPlayers = numberOfPlayers,
                    TotalPrice = room.PricePerPerson * numberOfPlayers
                });
            }

            _ctx.SaveChanges();
        }

        private void SeedReviews()
        {
            if (_ctx.Reviews.Any()) return;

            var faker = new Faker("en");
            var rooms = _ctx.Rooms.ToList();
            var users = _ctx.Users.Where(u => u.Role.Slug == "user").ToList();

            var roomsToReview = faker.Random.Shuffle(rooms.ToArray())
                .Take((int)Math.Ceiling(rooms.Count * 0.8))
                .ToList();

            foreach (var room in roomsToReview)
            {
                var reviewCount = faker.Random.Int(2, 5);
                var shuffledUsers = faker.Random.Shuffle(users.ToArray()).Take(reviewCount);

                foreach (var user in shuffledUsers)
                {
                    _ctx.Reviews.Add(new Review
                    {
                        UserId = user.Id,
                        RoomId = room.Id,
                        Rating = (byte)faker.Random.Int(1, 5),
                        Comment = faker.Random.Bool(0.8f) ? faker.Lorem.Sentence() : null
                    });
                }
            }

            _ctx.SaveChanges();
        }
    }
}
