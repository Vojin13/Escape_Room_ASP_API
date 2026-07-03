using Application.Commands.Users;
using Application.DTO.Users;
using Application.Exceptions;
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
    public class EfUpdateUserCommand : EfUseCase, IUpdateUserCommand
    {
        private UpdateUserValidator _validator;

        public EfUpdateUserCommand(AppDbContext context, UpdateUserValidator validator) : base(context)
        {
            _validator = validator;
        }

        public string Name => "Update User";

        public string Id => "update-user";

        public void Execute(UpdateUserDTO data)
        {
            _validator.ValidateAndThrow(data);

            var user = _ctx.Users.FirstOrDefault(x => x.Id == data.Id);

            if(user == null)
            {
                throw new NotFoundException("User", data.Id);
            }

            user.FirstName = data.FirstName;
            user.LastName = data.LastName;
            user.Username = data.Username;
            user.Email = data.Email;
            user.RoleId = data.RoleId;

            _ctx.SaveChanges();
        }
    }
}