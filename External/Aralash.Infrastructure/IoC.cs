namespace Aralash.Infrastructure;

public static class IoCConfig
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDatabase(configuration)
            .AddSecurityServices(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connStr = configuration.GetSection("ConnectionStrings")["PgsConnectionString"];
        services.AddDbContext<AralashDbContext>(opt =>
        {
            opt.UseNpgsql(connStr);
        });
        services.AddTransient<IAralashDbContext, AralashDbContext>();
        return services;
    }

    private static IServiceCollection AddSecurityServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<ITokenManager, TokenManager>();
        services.AddScoped<ISecuredOperationsManager, SecuredOperationsManager>();
        //TODO services.AddScoped<IRoleManager, RoleManager>();
        return services;
    }
}