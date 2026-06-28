using Domain.Entities;

namespace Domain
{
    public class UserUseCase
    {
        public int UserId { get; set; }
        public string UseCaseId { get; set; }

        public virtual User User { get; set; }
    }
}
