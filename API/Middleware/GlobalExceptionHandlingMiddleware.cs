using Application;
using Application.Exceptions;
using Domain.Entities;
using FluentValidation;
using Implementation;
using Sentry;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";

            if (ex is ValidationException e)
            {
                context.Response.StatusCode = 422;
                var errors = e.Errors.Select(x => new
                {
                    error = x.ErrorMessage,
                    property = x.PropertyName
                });
                await context.Response.WriteAsJsonAsync(errors);
                return;
            }

            if (ex is UnauthorizedUseCaseException)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
                return;
            }

            if (ex is InvalidCredentialsException)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
                return;
            }

            if (ex is NotFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
                return;
            }

            if (ex is ConflictException)
            {
                context.Response.StatusCode = 409;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
                return;
            }

            var supportCode = Guid.NewGuid();

            SentrySdk.CaptureException(ex, scope =>
            {
                scope.SetTag("support_code", supportCode.ToString());
            });

            var dbContext = context.RequestServices.GetRequiredService<AppDbContext>();
            var applicationUser = context.RequestServices.GetRequiredService<IApplicationUser>();

            dbContext.ErrorLogs.Add(new ErrorLog
            {
                SupportCode = supportCode,
                Message = ex.Message,
                File = ex.StackTrace,
                Url = context.Request.Path,
                Method = context.Request.Method,
                UserId = applicationUser.Id != 0 ? applicationUser.Id : null,
                CreatedAt = DateTime.UtcNow
            });
            await dbContext.SaveChangesAsync();

            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                message = $"An unexpected error occurred. Contact support with code: {supportCode}"
            });
        }
    }
}