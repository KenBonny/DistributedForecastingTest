using System.CommandLine;
using JasperFx.Core;
using JasperFx.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wolverine;
using Wolverine.RabbitMQ;

var (id, name) = ParseArgs(args);
if (id == -1) return;

var builder = Host.CreateDefaultBuilder(args);

var workerName = name ?? $"Worker {id}";
builder.ConfigureAppConfiguration((_, config) =>
{
    config.AddInMemoryCollection(
        new Dictionary<string, string> { ["Worker:Id"] = id.ToString(), ["Worker:Name"] = workerName }!);
});

builder.ConfigureServices((context, services) =>
{
    services.AddWolverine(options =>
    {
        options.ServiceName = workerName.Replace(' ', '-');
        options.Services.AddResourceSetupOnStartup();
        options.Policies.DisableConventionalLocalRouting();
        options.UseRabbitMq(context.Configuration.GetConnectionString("RabbitMQ")!)
            .UseConventionalRouting()
            .AutoProvision()
            .ConfigureListeners(listener => listener.MaximumParallelMessages(1));
    });
    services.Configure<WorkerIdentity>(context.Configuration.GetSection("Worker"));
    services.Configure<ForecastingEngine>(engine =>
        engine.Path = new FileInfo(context.Configuration.GetValue<string>("ForecastingEngine")));
});

var app = builder.Build();

await app.RunAsync();

(int id, string? name) ParseArgs(string[] args)
{
    var idOption = new Option<int>("--id") { Description = "The ID of the worker", Required = true};
    var nameOption = new Option<string>("--name") { Description = "The name of the worker", Required = false};

    var rootCommand = new RootCommand("The worker app to forecast stuff");
    rootCommand.Options.AddRange([idOption, nameOption]);
    var result = rootCommand.Parse(args);

    if (result.Errors.Count <= 0)
        return (result.GetRequiredValue(idOption), result.GetValue(nameOption));

    foreach (var error in result.Errors)
        Console.Error.WriteLine(error.Message);

    return (-1, "Unknown");
}

public record WorkerIdentity
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}

public record ForecastingEngine
{
    public required FileInfo Path { get; set; }
}