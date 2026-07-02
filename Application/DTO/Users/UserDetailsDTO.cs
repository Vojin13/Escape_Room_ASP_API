namespace Application.DTO.Users
{
    public class UserDetailsDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
