
namespace Loop54.Model.Response
{
    /// <summary>
    /// Response returned by the engine when an error has occured.
    /// </summary>
    public class ErrorResponse : Response
    {
        /// <summary>
        /// Details about the error.
        /// </summary>
        public ErrorDetails Error { get; set; }
    }
}
