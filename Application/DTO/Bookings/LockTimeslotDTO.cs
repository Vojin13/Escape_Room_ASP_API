namespace Application.DTO.Bookings
{
    public class LockTimeslotDTO
    {
        public int RoomId { get; set; }
        public int TimeslotId { get; set; }
        public DateTime Date { get; set; }
    }
}
