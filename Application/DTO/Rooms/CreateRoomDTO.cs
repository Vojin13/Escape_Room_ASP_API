using Microsoft.AspNetCore.Http;

namespace Application.DTO.Rooms
{
    public class CreateRoomDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public short MinimumPlayers { get; set; }
        public short MaximumPlayers { get; set; }
        public short DurationInMinutes { get; set; }
        public decimal PricePerPerson { get; set; }
        public int DifficultyId { get; set; }
        public List<int> TimeslotIds { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
