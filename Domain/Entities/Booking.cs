using Domain.Enums;

namespace Domain.Entities
{
    public class Booking : BaseEntity
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public int TimeslotId { get; set; }
        public int StatusId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int NumberOfPlayers { get; set; }

        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
        public virtual Timeslot Timeslot { get; set; }
        public virtual BookingStatusLookup Status { get; set; }
    }
}
