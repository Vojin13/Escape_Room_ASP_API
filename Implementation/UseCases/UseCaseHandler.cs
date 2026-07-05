using Application;
using Application.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Implementation.UseCases
{
    public class UseCaseHandler
    {

        private IApplicationUser _user;
        private AppDbContext _ctx;
        private IHttpContextAccessor _httpContextAccessor;

        public UseCaseHandler(IApplicationUser user, AppDbContext ctx, IHttpContextAccessor httpContextAccessor)
        {
            _user = user;
            _ctx = ctx;
            _httpContextAccessor = httpContextAccessor;
        }

        private void LogAttempt(IUseCase useCase, bool wasAuthorized, long? elapsedMs)
        {
            _ctx.AuditLogs.Add(new AuditLog
            {
                UserId = _user.Id != 0 ? _user.Id : null,
                UseCaseId = useCase.Id,
                UseCaseName = useCase.Name,
                WasAuthorized = wasAuthorized,
                Method = _httpContextAccessor.HttpContext?.Request?.Method,
                ElapsedMs = elapsedMs
            });
            _ctx.SaveChanges();
        }

        private void EnsureAuthorized(IUseCase useCase)
        {
            if (!_user.AllowedUseCases.Contains(useCase.Id))
            {
                LogAttempt(useCase, wasAuthorized: false, elapsedMs: null);
                throw new UnauthorizedUseCaseException(_user.Username, useCase.Name);
            }
        }

        public void ExecuteCommand<TRequest>(ICommand<TRequest> command, TRequest request)
        {
            EnsureAuthorized(command);

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                command.Execute(request);
            }
            finally
            {
                stopwatch.Stop();
                LogAttempt(command, wasAuthorized: true, elapsedMs: stopwatch.ElapsedMilliseconds);
            }

            Console.WriteLine($"{_user.Username} has executed use case: {command.Name}" + stopwatch.ElapsedMilliseconds + " ms.");
        }

        public TResult ExecuteQuery<TParam, TResult>(IQuery<TParam, TResult> query, TParam request)
            where TResult : class
        {
            EnsureAuthorized(query);

            Stopwatch stopwatch = Stopwatch.StartNew();
            TResult result;

            try
            {
                result = query.Execute(request);
            }
            finally
            {
                stopwatch.Stop();
                LogAttempt(query, wasAuthorized: true, elapsedMs: stopwatch.ElapsedMilliseconds);
            }

            Console.WriteLine($"{_user.Username} has executed use case: {query.Name}" + stopwatch.ElapsedMilliseconds + " ms.");

            return result;
        }
    }
}
