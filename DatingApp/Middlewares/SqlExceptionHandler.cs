using Microsoft.Data.SqlClient;
using System.Net;
using DatingApp.Enums;

namespace DatingApp.Middlewares
{
    /// <summary>
    /// Handler for <see cref="SqlException"/> errors.
    /// </summary>
    public static class SqlExceptionHandler
    {
        /// <summary>
        /// Handle exception.
        /// </summary>
        /// <param name="response">Response that will hold info about result of handling the exception.</param>
        /// <param name="exception">Exception to be handled.</param>
        /// <param name="handleErrorResponseAsync">
        /// Generate response according to provided HTTP status code (<see cref="HttpStatusCode"/>) and message (<see cref="string"/>), and put it into response object (<see cref="HttpResponse"/>).
        /// </param>
        public static async Task HandleExceptionAsync(HttpResponse response, SqlException exception, Func<HttpResponse, HttpStatusCode, string, Task> handleErrorResponseAsync)
        {
            switch (exception.Number)
            {
                case (int)SqlExceptionNumbers.DuplicatedKeyRowInObject:
                    await handleErrorResponseAsync(response, HttpStatusCode.BadRequest, "Cannot insert duplicate key row.");
                    break;

                case (int)SqlExceptionNumbers.DuplicatedKeyInObject:
                    await handleErrorResponseAsync(response, HttpStatusCode.BadRequest, "Violation of constraint, cannot insert duplicate key.");
                    break;

                case (int)SqlExceptionNumbers.DependentObjectExists:
                    await handleErrorResponseAsync(response, HttpStatusCode.BadRequest, "The statement conflicted with constraint.");
                    break;

                case (int)SqlExceptionNumbers.InvalidObjectName:
                    await handleErrorResponseAsync(response, HttpStatusCode.BadRequest, "Invalid object name (table or column name).");
                    break;

                case (int)SqlExceptionNumbers.TargetDatabaseNotAccessible:
                    await handleErrorResponseAsync(response, HttpStatusCode.InternalServerError, "Target database is not accessible for queries.");
                    break;

                case (int)SqlExceptionNumbers.DatabaseNotAllowedConnections:
                    await handleErrorResponseAsync(response, HttpStatusCode.InternalServerError, "Unable to access database.");
                    break;

                default:
                    await handleErrorResponseAsync(response, HttpStatusCode.InternalServerError, exception.Message);
                    break;
            }
        }
    }
}
