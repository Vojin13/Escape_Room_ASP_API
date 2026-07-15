namespace Application.DTO.Bookings
{
    public class MyCartDTO
    {
        public int LockId { get; set; }
        public int RoomId { get; set; }
        public string RoomTitle { get; set; }
        public string? PrimaryImage { get; set; }
        public int TimeslotId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime Date { get; set; }
        public decimal PricePerPerson { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
