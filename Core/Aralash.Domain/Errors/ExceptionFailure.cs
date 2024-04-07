namespace Aralash.Domain.Errors;

[Serializable]
public class ExceptionFailure
{
    public ExceptionFailure(Exception exception, string traceId, bool isDevelopment)
    {
        StatusCode = (int)HttpStatusCode.InternalServerError;
        StatusMessage = HttpStatusCode.InternalServerError.ToString();
        StackTrace = isDevelopment ?
            exception.StackTrace?.Split(Environment.NewLine, StringSplitOptions.None).ToList()
            : null;

        ErrorMessage = exception.GetBaseException().Message;
        TraceId = traceId;
    }

    public ExceptionFailure(int statusCode, string statusMessage, Exception exception, string traceId, bool isDevelopment)
    {
        StatusCode = statusCode;
        StatusMessage = statusMessage;
        ErrorMessage = exception.GetBaseException().Message;
        StackTrace = isDevelopment ?
            exception.StackTrace?.Split(Environment.NewLine, StringSplitOptions.None).ToList()
            : null;
        TraceId = traceId;
    }

    public ExceptionFailure(HttpStatusCode httpStatusCode, Exception exception, string traceId, bool isDevelopment)
        : this((int)httpStatusCode, httpStatusCode.ToString(), exception, traceId, isDevelopment)
    {
    }

    public ExceptionFailure()
    {
        StatusCode = 0;
        StatusMessage = string.Empty;
        ErrorMessage = string.Empty;
        StackTrace = null;
        TraceId = string.Empty;
    }

    public int StatusCode { get; set; }
    public string StatusMessage { get; set; }
    public string ErrorMessage { get; set; }
    public string TraceId { get; set; }
    [JsonProperty(PropertyName = "stackTrace")]
    public List<string>? StackTrace { get; set; }
}