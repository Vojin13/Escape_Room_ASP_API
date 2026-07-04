namespace Application.DTO.Search
{
    public class MyBookingSearchDTO : PagedSearch
    {
        public int UserId { get; set; }
        public int? StatusId { get; set; }
        public bool SortDescending { get; set; }
    }
}
