using Application;

namespace Tests.Common
{
    public class TestApplicationUser : IApplicationUser
    {
        public int Id { get; set; }
        public string Email { get; set; } = "test@test.com";
        public string Username { get; set; } = "testuser";
        public IEnumerable<string> AllowedUseCases { get; set; } = new List<string>();
    }
}
