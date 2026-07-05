namespace Application.DTO.Search
{
    public class ReviewSearchDTO : PagedSearch
    {
        public int RoomId { get; set; }
        public byte? Rating { get; set; }
    }
}
