namespace Application.DTO.Search
{
    public class RoomSearchDTO : PagedSearch
    {
        public string Title { get; set; }
        public int? DifficultyId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? PlayersCount { get; set; }
        public short? DurationInMinutes { get; set; }
    }
}
