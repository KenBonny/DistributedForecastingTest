using JasperFx.Resources;
using Marten;
using Wolverine;
using Wolverine.Http;
using Wolverine.Marten;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

builder.Services.AddWolverine(options =>
    {
        options.ServiceName = "Orchestrator";
        options.Services.AddResourceSetupOnStartup();
        options.UseRabbitMqUsingNamedConnection("RabbitMQ").UseConventionalRouting();
    })
    .AddWolverineHttp()
    .AddMarten(options =>
    {
        options.Connection(builder.Configuration.GetConnectionString("Postgres")!);
        options.DatabaseSchemaName = "orchestration";
    })
    .IntegrateWithWolverine()
    .UseNpgsqlDataSource();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi().AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapWolverineEndpoints();

app.Run();