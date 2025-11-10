using DatingApp.Exceptions;
using Microsoft.Data.SqlClient;
using FluentValidation;
using System.Net;

namespace DatingApp.Middlewares
{
    /// <summary>
    /// Custom implementation for middleware-like exception handling.
    /// </summary>
    public class ApplicationExceptionHandler : ExceptionHandlerMiddlewareBase
    {
        /// <summary>
        /// Creates Application Exception Handler.
        /// </summary>
        /// <param name="next">Request delegate.</param>
        public ApplicationExceptionHandler(RequestDelegate next) : base(next)
        {
        }

        /// <summary>
        /// Method that filters and handles exception.
        /// </summary>
        /// <param name="response">Http response.</param>
        /// <param name="exception">Concrete exception.</param>
        /// <returns>Populated Http response.</returns>
        public override async Task HandleExceptionAsync(HttpResponse response, Exception exception)
        {
            switch (exception)
            {
                case ValidationException validationException:
                    await HandleErrorResponseAsync(response, HttpStatusCode.BadRequest, validationException.Message);
                    break;

                case NotFoundException notFoundException:
                    await HandleErrorResponseAsync(response, HttpStatusCode.NotFound, notFoundException.Message);
                    break;

                case BadRequestException badRequestException:
                    await HandleErrorResponseAsync(response, HttpStatusCode.BadRequest, badRequestException.Message);
                    break;

                case ArgumentException argumentException:
                    await HandleErrorResponseAsync(response, HttpStatusCode.BadRequest, argumentException.Message);
                    break;

                case SqlException sqlException:
                    await SqlExceptionHandler.HandleExceptionAsync(response, (SqlException)exception, HandleErrorResponseAsync);
                    break;

                default:
                    await base.HandleExceptionAsync(response, exception);
                    break;
            }
        }

        /// <summary>
        /// Populates response with status code and error response.
        /// </summary>
        /// <param name="response">Response to be populated.</param>
        /// <param name="statusCode">Http status code.</param>
        /// <param name="message">Error message.</param>
        /// <returns>Populated Http response.</returns>
        private async Task HandleErrorResponseAsync(HttpResponse response, HttpStatusCode statusCode, string message)
        {
            response.StatusCode = (int)statusCode;
            await response.WriteAsync(SerializeResponse(new ErrorResponse { ErrorMessage = message }));
        }
    }
}
