namespace Application.DTO.Search
{
    public class BookingSearchDTO : PagedSearch
    {
        public string Keyword { get; set; }
        public int? StatusId { get; set; }
        public bool SortDescending { get; set; }
    }
}
