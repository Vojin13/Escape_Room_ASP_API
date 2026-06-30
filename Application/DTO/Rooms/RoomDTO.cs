namespace Application.DTO.Rooms
{
    public class RoomDTO
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
        public string PrimaryImage { get; set; }
    }
}
