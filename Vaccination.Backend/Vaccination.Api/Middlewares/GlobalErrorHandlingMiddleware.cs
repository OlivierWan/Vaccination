using System.Net;
using System.Text.Json;
using Vaccination.Api.Exceptions;
using Vaccination.Application.Dtos;
using Vaccination.Application.Exceptions;
using KeyNotFoundException = Vaccination.Application.Exceptions.KeyNotFoundException;
using NotImplementedException = Vaccination.Application.Exceptions.NotImplementedException;
using UnauthorizedAccessException = Vaccination.Api.Exceptions.UnauthorizedAccessException;

namespace Vaccination.Api.Middlewares
{
    public class GlobalErrorHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message;

            Type exceptionType = exception.GetType();

            if (exceptionType == typeof(Exceptions.DatabaseException))
            {
                status = HttpStatusCode.ServiceUnavailable;
                message = exception.Message;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                message = exception.Message;
                status = HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                status = HttpStatusCode.NotImplemented;
                message = exception.Message;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException) || 
                    (exceptionType == typeof(PasswordException)) || 
                    (exceptionType == typeof(SecurityTokenException)) || 
                    (exceptionType == typeof(TokenException)))
            {
                status = HttpStatusCode.Unauthorized;
                message = exception.Message;
            }
            else if (exceptionType == typeof(KeyNotFoundException))
            {
                status = HttpStatusCode.NotFound;
                message = exception.Message;
            }
            else if (exceptionType == typeof(Application.Exceptions.DatabaseException) || 
                    (exceptionType == typeof(Infrastructure.Exceptions.DatabaseException)))
            {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
            }
            else if (exceptionType == typeof(InvalidOperationException) || 
                    (exceptionType == typeof(Infrastructure.Exceptions.DuplicateDataException)) || 
                    (exceptionType == typeof(Application.Exceptions.DuplicateDataException)))
            {
                status = HttpStatusCode.Conflict;
                message = exception.Message;
            }
            else if (exceptionType == typeof(PaginationException))
            {
                status = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else if (exceptionType == typeof(FluentValidation.ValidationException))
            {

                status = HttpStatusCode.BadRequest;

                FluentValidation.ValidationException validationException = (FluentValidation.ValidationException)exception;
                message = JsonSerializer.Serialize(validationException.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }));
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
            }

            string exceptionResult = JsonSerializer.Serialize(new ApiResponse<string>() { Message = message, Success = false });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(exceptionResult);
        }
    }
}