using System.Diagnostics;
using Shared;

namespace WolverineTest.Worker.Features;

public class WeeklyForecast
{
    public static async Task<WeeklyForecastResponse> Handle(WeeklyForecastRequest request, ForecastIdentity identity)
    {
        var now = DateTimeOffset.Now;
        var stopwatch = Stopwatch.StartNew();

        // Simulate work
        await Task.Delay(request.Duration);

        stopwatch.Stop();
        return new WeeklyForecastResponse(
            request.Day,
            identity.Id,
            identity.Name,
            now,
            stopwatch.Elapsed);
    }
}