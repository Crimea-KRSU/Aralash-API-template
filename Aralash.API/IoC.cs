namespace Aralash.API;

public static class IoCConfig
{
    public static IServiceCollection ConfigureWebApi(this IServiceCollection collection, IConfiguration configuration)
    {
        collection
            .AddControllers()
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
        
        collection
            .AddCorsPolicy()
            .AddEndpointsApiExplorer()
            .ConfigureSwagger()
            .AddApiAuthentication(configuration)
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
            .AddScoped<ICurrentUser, CurrentUser>();
        
        return collection;
    }

    private static IServiceCollection ConfigureSwagger(this IServiceCollection collection)
    {
        collection.AddSwaggerGen(c => {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        return collection;
    }
    
    private static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder => builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            )
        );

        return services;
    }
    
    private static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = new JwtOptions();
        configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.JwtAccessSecretKey)),
                    LifetimeValidator = (before, expires, token, parameters) =>
                    {
                        if (expires == null) return false; // Expired
                        return DateTime.UtcNow < expires.Value.ToUniversalTime();
                    },
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Headers.Authorization;
                        if (string.IsNullOrEmpty(token) || token.ToString().Split()[0] != "Bearer" ||
                            token.ToString().Split().Length < 2)
                            context.Token = string.Empty;
                        else context.Token = token.ToString().Split()[1];

                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
    
    public static void ScanSecuredOperations(this WebApplication app, 
        Assembly assembly)
    {
        using var scope = app.Services.CreateScope();
        var spManager = scope.ServiceProvider.GetService<ISecuredOperationsManager>();
        if (spManager == null)
            throw new ArgumentException($"Отсутствует реализация интерфейса {nameof(ISecuredOperationsManager)}");
        spManager.ScanAndUpdateSecuredOperations(assembly);
    }
}