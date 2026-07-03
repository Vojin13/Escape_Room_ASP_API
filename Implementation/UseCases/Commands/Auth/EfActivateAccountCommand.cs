using Application.Commands.Auth;
using Application.Emails;
using Application.Exceptions;
using Implementation.Emails;

namespace Implementation.UseCases.Commands.Auth
{
    public class EfActivateAccountCommand : EfUseCase, IActivateAccountCommand
    {
        private readonly IEmailSender _sender;
        private readonly EmailTemplateComposer _composer;

        public EfActivateAccountCommand(AppDbContext context, IEmailSender sender, EmailTemplateComposer composer)
            : base(context)
        {
            _sender = sender;
            _composer = composer;
        }

        public string Name => "Account activation";

        public string Id => "account-activate";

        public void Execute(string data)
        {
            if (!Guid.TryParse(data, out var code))
                throw new NotFoundException("User", data);

            var user = _ctx.Users.FirstOrDefault(x => x.EmailVerificationToken == code);

            if (user == null)
                throw new NotFoundException("User", data);

            if (user.EmailVerifiedAt.HasValue)
                throw new NotFoundException("User", data);

            if ((DateTime.UtcNow - user.CreatedAt).TotalDays > 1)
                throw new NotFoundException("User", data);

            user.EmailVerifiedAt = DateTime.UtcNow;
            user.EmailVerificationToken = null;

            var html = _composer.GetTemplateContent(EmailTemplate.Activation, user);
            _sender.SendEmail(user.Email, "Account activated", html);

            _ctx.SaveChanges();
        }
    }
}
