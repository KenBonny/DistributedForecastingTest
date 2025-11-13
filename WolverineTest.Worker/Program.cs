using System.CommandLine;
using JasperFx.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wolverine;
using Wolverine.RabbitMQ;

var (id, name) = ParseArgs(args);
if (id == -1) return;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddWolverine(options =>
    {
        options.ServiceName = name ?? $"Worker {id}";
        options.UseRabbitMqUsingNamedConnection("RabbitMQ").UseConventionalRouting();
    });
    services.AddSingleton(new ForecastIdentity(id, name ?? $"Worker {id}"));
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

public record ForecastIdentity(int Id, string Name);