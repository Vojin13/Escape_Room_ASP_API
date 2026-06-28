using Domain;

namespace Implementation.UseCases
{
    public abstract class EfUseCase
    {
        protected readonly AppDbContext _ctx;

        protected EfUseCase(AppDbContext context)
        {
            _ctx = context;
        }

        protected void AssignRoleUseCases(int userId, int roleId)
        {
            var roleUseCases = _ctx.RoleUseCases
                .Where(r => r.RoleId == roleId)
                .ToList();

            _ctx.UserUseCases.AddRange(roleUseCases.Select(uc => new UserUseCase
            {
                UserId = userId,
                UseCaseId = uc.UseCaseId
            }));

            _ctx.SaveChanges();
        }

    }
}
