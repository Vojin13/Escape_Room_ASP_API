using Application.DTO;
using Application.DTO.Search;
using Application.DTO.Users;
using Application.Extensions;
using Application.Queries.Users.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.UseCases.Queries.Users.Admin
{
    internal class EfAdminGetUsersQuery : EfUseCase, IAdminGetUsersQuery
    {
        public EfAdminGetUsersQuery(AppDbContext context) : base(context)
        {
        }

        public string Name => "Admin Get Users";

        public string Id => "admin-get-users";

        public PagedResponse<UserDTO> Execute(UserSearchDTO request)
        {
            var query = _ctx.Users.Where(x => !x.IsDeleted).OrderBy(x => x.Id).AsQueryable();

            if(!string.IsNullOrEmpty(request.Keyword))
            { 
                query = query.WhereContainsIgnoreCaseAny(request.Keyword, x => x.Username, x => x.Email);
            }

            if (request.RoleId.HasValue)
            {
                query = query.Where(x => x.RoleId == request.RoleId.Value);
            }

            return query.Select(x => new UserDTO
            {
                Id = x.Id,
                Username = x.Username,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Role = x.Role.Name,
                EmailVerified = x.EmailVerifiedAt.HasValue
            }).ToPagedResponse(request.Page, request.PerPage);
        }
    }
}
