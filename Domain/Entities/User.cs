namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public Guid? EmailVerificationToken { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Role Role { get; set; }
        public virtual HashSet<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public virtual HashSet<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual HashSet<TimeslotLock> TimeslotLocks { get; set; } = new HashSet<TimeslotLock>();
        public virtual HashSet<AuthToken> AuthTokens { get; set; } = new HashSet<AuthToken>();
        public virtual HashSet<UserUseCase> UserUseCases { get; set; } = new HashSet<UserUseCase>();
    }
}
