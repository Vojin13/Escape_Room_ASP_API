using Application;
using Domain.Entities;

namespace Implementation
{
    public class UnauthorizedUser : IApplicationUser
    {
        public int Id => 0;

        public string Email => "guest@gmail.com";

        public string Username => "unauthorized";

        public IEnumerable<string> AllowedUseCases =>
            new List<string> { "register-user", "login", "refresh-token", "get-rooms", "get-room", "get-room-availability",
            "seed", "create-room", "update-room", "admin-get-rooms", "admin-get-room", "admin-get-users", "create-user",
                "update-user", "admin-delete-user", "delete-user", "admin-get-user",
                "get-timeslots", "get-timeslot", "create-timeslot", "update-timeslot", "delete-timeslot", "toggle-room-active",
                "account-activate" };
    }
}
