namespace Aralash.Domain.Primitives;

[Serializable]
public class Failure
{
    [JsonProperty("failureInfo")]
    public string Message { get; init; } = string.Empty;

    [JsonProperty("traceId")] 
    public string TraceId { get; init; } = string.Empty;
}