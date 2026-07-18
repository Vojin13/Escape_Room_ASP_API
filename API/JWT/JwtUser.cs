using Application;
using System.Collections.Generic;

namespace ASPLAB2.API.JWT
{
    public class JwtUser : IApplicationUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set;}
        public IEnumerable<string> AllowedUseCases { get; set; } = new List<string>();
    }
}
