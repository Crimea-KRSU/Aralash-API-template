namespace Aralash.Domain.Errors;

public class AuthFailure : Failure
{
    public AuthFailure(string info, string traceId)
    {
        Message = info;
        TraceId = traceId;
    }
}