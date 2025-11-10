using System.Net;
using System.Text.Json;

namespace DatingApp.Middlewares
{
    /// <summary>
    /// Base implementation of exception handler using middleware.
    /// </summary>
    public class ExceptionHandlerMiddlewareBase
    {
        /// <summary>
        /// Request delegate.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="next">Request delegate.</param>
        public ExceptionHandlerMiddlewareBase(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Method that will be invoked my middleware.
        /// </summary>
        /// <param name="context">Http context.</param>
        /// <returns>Http response.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(response, exception);
            }
        }

        /// <summary>
        /// Method that handle basic exceptions.
        /// </summary>
        /// <param name="response">Http response.</param>
        /// <param name="exception">Concrete exception.</param>
        /// <returns>Populates Http response object.</returns>
        public virtual async Task HandleExceptionAsync(HttpResponse response, Exception exception)
        {
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await response.WriteAsync(SerializeResponse(new ErrorResponse { ErrorMessage = exception.Message }));
        }

        /// <summary>
        /// Method that serialize response.
        /// </summary>
        /// <param name="errorResponse">Response to be serialized.</param>
        /// <returns>Serialized response.</returns>
        public string SerializeResponse(ErrorResponse errorResponse)
        {
            return JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
