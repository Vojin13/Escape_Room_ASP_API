namespace Application.DTO.Search
{
    public class UserSearchDTO : PagedSearch
    {
        public string Keyword { get; set; }
        public int? RoleId { get; set; }
    }
}
