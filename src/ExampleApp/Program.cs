using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("mssql"),
        healthQuery: "SELECT 1",
        name: "MS SQL Server Check",
        failureStatus: HealthStatus.Unhealthy | HealthStatus.Degraded,
        tags: new string[] { "db", "sql", "sqlserver" })

    .AddRedis(
        redisConnectionString: builder.Configuration.GetConnectionString("redis"),
        name: "Redis Check",
        failureStatus: HealthStatus.Unhealthy | HealthStatus.Degraded,
        tags: new string[] { "redis" });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHealthChecks("/hc", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.Run();
