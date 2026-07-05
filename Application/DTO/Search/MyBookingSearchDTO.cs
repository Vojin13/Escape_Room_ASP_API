namespace Application.DTO.Search
{
    public class MyBookingSearchDTO : PagedSearch
    {
        public int? StatusId { get; set; }
        public bool SortDescending { get; set; }
    }
}
