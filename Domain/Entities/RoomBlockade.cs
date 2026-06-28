namespace Domain.Entities
{
    public class RoomBlockade : BaseEntity
    {
        public int RoomId { get; set; }
        public int TimeslotId { get; set; }
        public string Reason { get; set; }
        public DateTime BlockadeDate { get; set; }

        public virtual Room Room { get; set; }
        public virtual Timeslot Timeslot { get; set; }
    }
}
