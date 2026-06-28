namespace Domain.Entities
{
    public class BookingStatusLookup : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual HashSet<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}
