namespace Domain.Entities
{
    public class Room : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public short MinimumPlayers { get; set; }
        public short MaximumPlayers { get; set; }
        public short DurationInMinutes { get; set; }
        public bool IsActive { get; set; }
        public int DifficultyId { get; set; }
        public decimal PricePerPerson { get; set; }

        public virtual Difficulty Difficulty { get; set; }
        public virtual HashSet<RoomImage> Images { get; set; } = new HashSet<RoomImage>();
        public virtual HashSet<RoomTimeslot> RoomTimeslots { get; set; } = new HashSet<RoomTimeslot>();
        public virtual HashSet<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public virtual HashSet<Review> Reviews { get; set; } = new HashSet<Review>();
    }
}
