namespace Loop54.Model.Response
{
    /// <summary>
    /// Detailed information regarding an error returned from the engine.
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The name of the error.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The more detailed information about the error. Note: not always shown.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// The input parameter, if any, that caused the error.
        /// </summary>
        public string Parameter { get; set; }
    }
}
