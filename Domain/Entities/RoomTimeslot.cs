namespace Domain.Entities
{
    public class RoomTimeslot
    {
        public int RoomId { get; set; }
        public int TimeslotId { get; set; }

        public virtual Room Room { get; set; }
        public virtual Timeslot Timeslot { get; set; }
    }
}
