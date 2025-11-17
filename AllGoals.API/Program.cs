using AllGoals.Application.Interfaces;
using AllGoals.Application.Services;
using AllGoals.Domain.Interfaces;
using AllGoals.Infrastructure.Data;
using AllGoals.Infrastructure.Repositories;
using Asp.Versioning;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog; 

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Iniciando a API AllGoals...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddOpenTelemetry()
        .WithTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("AllGoals.API"))
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddConsoleExporter();
        });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
//    builder.Services.AddSwaggerGen();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
        { 
            Title = "AllGoals API - V1", 
            Version = "v1",
            Description = "Versão estável básica."
        });

        options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo 
        { 
            Title = "AllGoals API - V2", 
            Version = "v2",
            Description = "Versão avançada com gestão de administradores."
        });

        options.CustomSchemaIds(type => type.ToString());
    });

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddHealthChecks()
        .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy())
        .AddOracle(connectionString!, name: "oracle-db", tags: new[] { "db", "data" });

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseOracle(connectionString, oraOptions => 
        {

        })
    );

    builder.Services.AddScoped<IGoalRepository, GoalRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IStoreItemRepository, StoreItemRepository>();

    builder.Services.AddScoped<IGoalService, GoalService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IStoreItemService, StoreItemService>();
    
    builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);

            options.AssumeDefaultVersionWhenUnspecified = true;

            options.ReportApiVersions = true;

            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";

            options.SubstituteApiVersionInUrl = true;
        });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options => 
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "AllGoals API v1");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "AllGoals V2");
        });
    }

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();

    app.UseAuthorization();

    app.MapControllers();

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = _ => true
    });

    app.MapHealthChecks("/health/details", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação falhou ao iniciar inesperadamente");
}
finally
{
    Log.CloseAndFlush();
}