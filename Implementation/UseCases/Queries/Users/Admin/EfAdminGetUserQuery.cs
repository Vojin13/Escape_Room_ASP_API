using Application.DTO.Users;
using Application.Exceptions;
using Application.Queries.Users.Admin;
using Microsoft.EntityFrameworkCore;

namespace Implementation.UseCases.Queries.Users.Admin
{
    public class EfAdminGetUserQuery : EfUseCase, IAdminGetUserQuery
    {
        public EfAdminGetUserQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Admin Get User Info";

        public string Id => "admin-get-user";

        public UserDetailsDTO Execute(int request)
        {
            var user = _ctx.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Id == request && !x.IsDeleted);

            if(user == null)
            {
                throw new NotFoundException("User", request);
            }

            return new UserDetailsDTO
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.Name,
                EmailVerifiedAt = user.EmailVerifiedAt,
                RoleId = user.RoleId,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
