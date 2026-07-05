using Application;
using Application.Exceptions;
using Domain.Entities;
using System.Diagnostics;

namespace Implementation.UseCases
{
    public class UseCaseHandler
    {

        private IApplicationUser _user;
        private AppDbContext _ctx;

        public UseCaseHandler(IApplicationUser user, AppDbContext ctx)
        {
            _user = user;
            _ctx = ctx;
        }
        private void HandleAuthorization(IUseCase useCase)
        {
            bool isAuthorized = _user.AllowedUseCases.Contains(useCase.Id);

            _ctx.AuditLogs.Add(new AuditLog
            {
                UserId = _user.Id != 0 ? _user.Id : null,
                UseCaseId = useCase.Id,
                UseCaseName = useCase.Name,
                WasAuthorized = isAuthorized
            });
            _ctx.SaveChanges();

            if (!isAuthorized)
            {
                throw new UnauthorizedUseCaseException(_user.Username, useCase.Name);
            }
        }
        public void ExecuteCommand<TRequest>(ICommand<TRequest> command, TRequest request)
        {
            HandleAuthorization(command);

            Stopwatch stopwatch = Stopwatch.StartNew();

            stopwatch.Start();

            command.Execute(request);

            stopwatch.Stop();

            Console.WriteLine($"{_user.Username} has executed use case: {command.Name}" + stopwatch.ElapsedMilliseconds + " ms.");
        }

        public TResult ExecuteQuery<TParam, TResult>(IQuery<TParam, TResult> query, TParam request)
            where TResult : class
        {
            HandleAuthorization(query);
            Stopwatch stopwatch = Stopwatch.StartNew();

            stopwatch.Start();

            var result = query.Execute(request);

            stopwatch.Stop();

            Console.WriteLine($"{_user.Username} has executed use case: {query.Name}" + stopwatch.ElapsedMilliseconds + " ms.");

            return result;
        }
    }
}
