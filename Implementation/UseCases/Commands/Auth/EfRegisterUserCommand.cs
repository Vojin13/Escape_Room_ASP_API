using Application.Commands.Auth;
using Application.DTO.Auth;
using Application.Emails;
using Domain.Entities;
using FluentValidation;
using Implementation.Emails;
using Implementation.UseCases.Validators.Auth;

namespace Implementation.UseCases.Commands.Auth
{
    public class EfRegisterUserCommand : EfUseCase, IRegisterUserCommand
    {
        private readonly RegisterUserValidator _validator;
        private readonly IEmailSender _emailSender;
        private readonly EmailTemplateComposer _composer;

        public EfRegisterUserCommand(AppDbContext context, RegisterUserValidator validator, IEmailSender emailSender, EmailTemplateComposer composer)
            : base(context)
        {
            _validator = validator;
            _emailSender = emailSender;
            _composer = composer;
        }

        public string Name => "User registration";

        public string Id => "register-user";

        public void Execute(RegisterUserDTO data)
        {
            _validator.ValidateAndThrow(data);

            var userRole = _ctx.Roles.First(x => x.Slug == "user");

            string hash = BCrypt.Net.BCrypt.HashPassword(data.Password);

            User user = new User();
            user.Username = data.Username;
            user.Password = hash;
            user.Email = data.Email;
            user.FirstName = data.FirstName;
            user.LastName = data.LastName;
            user.RoleId = userRole.Id;
            user.EmailVerificationToken = Guid.NewGuid();

            _ctx.Users.Add(user);
            _ctx.SaveChanges();

            AssignRoleUseCases(user.Id, userRole.Id);

            var activationLink = $"http://localhost:4200/verify-email?code={user.EmailVerificationToken}";

            var html = _composer.GetTemplateContent(EmailTemplate.Register, new
            {
                firstName = user.FirstName,
                activationLink,
                supportEmail = "support@cipherescape.com"
            });

            _emailSender.SendEmail(user.Email, "Confirm your CIPHER ESCAPE account", html);
        }
    }
}
