using Application.DTO.Timeslot;

namespace Application.DTO.Rooms
{
    public class RoomDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public short MinimumPlayers { get; set; }
        public short MaximumPlayers { get; set; }
        public short DurationInMinutes { get; set; }
        public decimal PricePerPerson { get; set; }
        public string Difficulty { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public List<string> Images { get; set; }
        public List<TimeslotDTO> Timeslots { get; set; }
    }
}
