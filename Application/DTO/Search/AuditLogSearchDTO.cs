namespace Application.DTO.Search
{
    public class AuditLogSearchDTO : PagedSearch
    {
        public string? Keyword { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool SortDescending { get; set; }
    }
}
