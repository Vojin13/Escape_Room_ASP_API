namespace Domain.Entities
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public Guid SupportCode { get; set; }
        public string Message { get; set; }
        public string? File { get; set; }
        public int? Line { get; set; }
        public string? Url { get; set; }
        public string? Method { get; set; }
        public string? Payload { get; set; }
        public int? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual User? User { get; set; }
    }
}
