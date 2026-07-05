namespace Application.DTO.AuditLogs
{
    public class AuditLogDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string UseCaseId { get; set; }
        public string UseCaseName { get; set; }
        public bool WasAuthorized { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
