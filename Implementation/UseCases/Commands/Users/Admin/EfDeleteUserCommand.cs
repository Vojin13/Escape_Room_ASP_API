using Application.Commands.Users;
using Application.Exceptions;
using System.Linq;

namespace Implementation.UseCases.Commands.Users.Admin
{
    public class EfDeleteUserCommand : EfUseCase, IDeleteUserCommand
    {
        public EfDeleteUserCommand(AppDbContext context) : base(context)
        {
        }

        public string Name => "Delete User";

        public string Id => "delete-user";

        public void Execute(int data)
        {
            var user = _ctx.Users.FirstOrDefault(x => x.Id == data && !x.IsDeleted);

            if(user == null)
            {
                throw new NotFoundException("User", data);
            }

            user.IsDeleted = true;
            _ctx.SaveChanges();
        }
    }
}
