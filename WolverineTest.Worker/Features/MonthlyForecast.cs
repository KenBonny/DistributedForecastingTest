using System.Diagnostics;
using Microsoft.Extensions.Options;
using Shared;

namespace WolverineTest.Worker.Features;

public class MonthlyForecastHandler
{
    public static async Task<MonthlyForecastResponse> Handle(MonthlyForecastRequest request, 
        IOptions<WorkerIdentity> identity,
        IOptions<FileInfo> forecastEngine)
    {
        var now = DateTimeOffset.Now;
        var stopwatch = Stopwatch.StartNew();

        // Simulate work
        await Task.Delay(request.Duration);

        stopwatch.Stop();
        return new MonthlyForecastResponse(
            request.Day,
            identity.Value.Id,
            identity.Value.Name,
            now,
            stopwatch.Elapsed);
    }
}