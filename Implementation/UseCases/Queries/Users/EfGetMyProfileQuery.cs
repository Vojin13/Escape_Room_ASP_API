using Application;
using Application.DTO;
using Application.DTO.Users;
using Application.Exceptions;
using Application.Queries.Users;
using Microsoft.EntityFrameworkCore;

namespace Implementation.UseCases.Queries.Users
{
    public class EfGetMyProfileQuery : EfUseCase, IGetMyProfileQuery
    {
        private readonly IApplicationUser _currentUser;

        public EfGetMyProfileQuery(AppDbContext context, IApplicationUser currentUser) : base(context)
        {
            _currentUser = currentUser;
        }

        public string Name => "Get My Profile";

        public string Id => "get-my-profile";

        public UserDetailsDTO Execute(NoData data)
        {
            var user = _ctx.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Id == _currentUser.Id);

            if (user == null)
                throw new NotFoundException("User", _currentUser.Id);

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
