namespace Application.DTO.Bookings
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomTitle { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int TimeslotId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumberOfPlayers { get; set; }
        public decimal TotalPrice { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
