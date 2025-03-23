using LingoMQ.Presenters.WebApi;
using LingoMQ.Presenters.WebApi.Middlewares;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string localAllowSpecificOrigins = "lan_allow_specific_origins";

        builder.Configuration.AddEnvironmentVariables();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers().ConfigureControllers();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSwagger();
        builder.Services.AddJwtAuth(builder.Configuration);
        builder.Services.ConfigureApplication();
        builder.Services.AddUsersFeatures(builder.Configuration);
        builder.Services.AddWordsFeatures();
        builder.Services.AddPersistense(builder.Configuration);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                localAllowSpecificOrigins,
                policy =>
                {
                    policy
                        //policy.WithOrigins("https://192.168.0.101:9000", "https://localhost:9000", "https://localhost")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowed((hosts) => true);
                }
            );
        });
        builder.Services.AddCors();

        var app = builder.Build();
        // app.Services.UsersInitializeDatabaseByEf();
        // app.Services.WordsInitializeDatabaseByEf();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseCors(localAllowSpecificOrigins);
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.MapControllers();

        app.Run();
    }
}
