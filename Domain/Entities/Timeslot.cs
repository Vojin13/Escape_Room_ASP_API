namespace Domain.Entities
{
    public class Timeslot : BaseEntity
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public virtual HashSet<RoomTimeslot> RoomTimeslots { get; set; } = new HashSet<RoomTimeslot>();
        public virtual HashSet<TimeslotLock> TimeslotLocks { get; set; } = new HashSet<TimeslotLock>();
    }
}
