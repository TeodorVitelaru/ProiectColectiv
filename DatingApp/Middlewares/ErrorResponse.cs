namespace DatingApp.Middlewares
{
    /// <summary>
    /// Represents Error Response.
    /// </summary>
    public sealed class ErrorResponse
    {
        /// <summary>
        /// Construct error response.
        /// </summary>
        public ErrorResponse()
        {
        }

        /// <summary>
        /// Creates error response with error message.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        public ErrorResponse(string? errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Error message.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
