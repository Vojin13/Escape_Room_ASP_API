namespace Domain.Entities
{
    public class TimeslotLock : BaseEntity
    {
        public int RoomId { get; set; }
        public int TimeslotId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime ExpiresAt { get; set; }

        public virtual Room Room { get; set; }
        public virtual Timeslot Timeslot { get; set; }
        public virtual User User { get; set; }
    }
}
