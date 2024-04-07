using Aralash.App.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Aralash.App;

public static class IocConfig
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IocConfig).Assembly));
        services.AddValidatorsFromAssembly(typeof(IocConfig).Assembly);
        services.AddAutoMapper(typeof(IocConfig).Assembly);
        services.ConfigureBehaviors();
        return services;
    }

    private static IServiceCollection ConfigureBehaviors(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ClaimsValidationBehavior<,>));
        return services;
    }
}