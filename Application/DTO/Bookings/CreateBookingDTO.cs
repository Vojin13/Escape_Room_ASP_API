namespace Application.DTO.Bookings
{
    public class CreateBookingDTO
    {
        public int RoomId { get; set; }
        public int TimeslotId { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfPlayers { get; set; }
    }
}
