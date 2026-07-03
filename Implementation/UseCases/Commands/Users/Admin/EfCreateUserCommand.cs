using Application.Commands.Users;
using Application.DTO.Users;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Implementation.UseCases.Validators.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.UseCases.Commands.Users.Admin
{
    public class EfCreateUserCommand : EfUseCase, ICreateUserCommand
    {
        private readonly CreateUserValidator _validator;
        private readonly IMapper _mapper;

        public EfCreateUserCommand(AppDbContext context,
                                   CreateUserValidator validator,
                                   IMapper mapper) : base(context)
        {
            _validator = validator;
            _mapper = mapper;
        }

        public string Name => "Admin Create User";

        public string Id => "create-user";

        public void Execute(CreateUserDTO data)
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
