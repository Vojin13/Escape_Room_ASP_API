namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public string Slug { get; set; }

        public virtual HashSet<User> Users { get; set; } = new HashSet<User>();
        public virtual HashSet<RoleUseCase> RoleUseCases { get; set; } = new HashSet<RoleUseCase>();
    }
}
