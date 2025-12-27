using System.Diagnostics;
using Microsoft.Extensions.Options;
using Shared;

namespace WolverineTest.Worker.Features;

public class WeeklyForecastHandler
{
    public static async Task<WeeklyForecastResponse> Handle(
        WeeklyForecastRequest request,
        IOptions<WorkerIdentity> identity,
        IOptions<ForecastingEngine> forecastEngine)
    {
        var now = DateTimeOffset.Now;
        var stopwatch = Stopwatch.StartNew();

        // Simulate work
        await Task.Delay(request.Duration);

        stopwatch.Stop();
        return new WeeklyForecastResponse(
            request.Day,
            identity.Value.Id,
            identity.Value.Name,
            now,
            stopwatch.Elapsed);
    }
}