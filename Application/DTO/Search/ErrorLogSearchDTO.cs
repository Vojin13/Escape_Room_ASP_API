namespace Application.DTO.Search
{
    public class ErrorLogSearchDTO : PagedSearch
    {
        public string? Code { get; set; }
        public string? Keyword { get; set; }
        public bool SortDescending { get; set; }
    }
}
