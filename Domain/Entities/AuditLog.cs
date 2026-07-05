namespace Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        public int? UserId { get; set; }
        public string UseCaseId { get; set; }
        public string UseCaseName { get; set; }
        public bool WasAuthorized { get; set; }

        public virtual User? User { get; set; }
    }
}
