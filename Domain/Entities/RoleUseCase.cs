namespace Domain.Entities
{
    public class RoleUseCase
    {
        public int RoleId { get; set; }
        public string UseCaseId { get; set; }

        public virtual Role Role { get; set; }
    }
}
