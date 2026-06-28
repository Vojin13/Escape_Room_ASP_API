using Application.Commands.Auth;
using Application.DTO.Auth;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Implementation.UseCases.Validators.Auth;

namespace Implementation.UseCases.Commands.Auth
{
    public class EfRegisterUserCommand : EfUseCase, IRegisterUserCommand
    {
        private readonly RegisterUserValidator _validator;
        private readonly IMapper _mapper;

        public string Name => "Register User";

        public string Id => "register-user";

        public EfRegisterUserCommand(AppDbContext ctx, RegisterUserValidator validator, IMapper mapper)
            : base(ctx)
        {
            _validator = validator;
            _mapper = mapper;
        }

        public void Execute(RegisterUserDTO data)
        {
            _validator.ValidateAndThrow(data);

            var userRole = _ctx.Roles.First(x => x.Slug == "user");

            var user = _mapper.Map<User>(data);
            user.Password = BCrypt.Net.BCrypt.HashPassword(data.Password);
            user.RoleId = userRole.Id;

            _ctx.Users.Add(user);
            _ctx.SaveChanges();

            AssignRoleUseCases(user.Id, userRole.Id);
        }
    }
}
