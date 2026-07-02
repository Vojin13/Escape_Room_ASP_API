using Application.DTO.Users;
using Application.Exceptions;
using Application.Queries.Users;
using Microsoft.EntityFrameworkCore;

namespace Implementation.UseCases.Queries.Users
{
    public class EfGetMyProfileQuery : EfUseCase, IGetMyProfileQuery
    {
        public EfGetMyProfileQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Get My Profile";

        public string Id => "get-my-profile";

        public UserDetailsDTO Execute(int userId)
        {
            var user = _ctx.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Id == userId);

            if (user == null)
                throw new NotFoundException("User", userId);

            return new UserDetailsDTO
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailVerifiedAt = user.EmailVerifiedAt,
                RoleId = user.RoleId,
                Role = user.Role.Name,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
