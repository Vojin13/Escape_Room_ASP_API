namespace Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        public int? UserId { get; set; }
        public string UseCaseId { get; set; }
        public string UseCaseName { get; set; }
        public bool WasAuthorized { get; set; }
        public string? Method { get; set; }
        public long? ElapsedMs { get; set; }

        public virtual User? User { get; set; }
    }
}
