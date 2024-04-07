using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace Aralash.API.Middlewares;

public class ExceptionMiddleware
{
    private const string JsonContentType = "application/problem+json";
    
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var traceId = Guid.NewGuid().ToString();

            _logger.LogError(exception, exception.Message + $"\nTraceId: ({traceId})");
            object errorResult;
            int httpStatusCode;
            switch (exception)
            {
                case ValidationException validationException:
                    errorResult = new BadRequestFailure(validationException.Errors, "Одна или несколько ошибок произошли при валидации запроса", traceId);
                    httpStatusCode = 400;
                    break;
                case AuthenticationException authenticationException:
                    errorResult = new AuthFailure(authenticationException.Message, traceId);
                    httpStatusCode = 401;
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    errorResult = new AuthFailure(unauthorizedAccessException.Message, traceId);
                    httpStatusCode = 403;
                    break;
                case ArgumentException:
                    errorResult = new BadRequestFailure(new List<ValidationFailure>(0), exception.Message, traceId);
                    httpStatusCode = 400;
                    break;
                default:
                    var apiException = new ExceptionFailure(exception, traceId, _env.IsDevelopment());
                    httpStatusCode = apiException.StatusCode;
                    errorResult = apiException;
                    break;
            }

            context.Response.StatusCode = httpStatusCode;
            context.Response.ContentType = JsonContentType;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(
                errorResult,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented,
                    StringEscapeHandling = StringEscapeHandling.Default
                }));
        }
    }
}