using FluentValidation.Results;

namespace Aralash.Domain.Errors;

public class BadRequestFailure : Failure
{
    public IEnumerable<ValidationFailure> ValidationFailures { get; init; }
    
    public BadRequestFailure(IEnumerable<ValidationFailure> validationFailures, string message, string traceId)
    {
        ValidationFailures = validationFailures;
        Message = message;
        TraceId = traceId;
    }
}