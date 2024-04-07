using Aralash.API.Middlewares;

namespace Aralash.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services
            .ConfigureWebApi(builder.Configuration)
            .ConfigureInfrastructure(builder.Configuration)
            .ConfigureApplication();
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        
        app.ScanSecuredOperations(typeof(App.AssemblyReference).Assembly);
        app.Run();
    }
}