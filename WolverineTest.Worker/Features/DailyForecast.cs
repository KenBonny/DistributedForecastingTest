using System.Diagnostics;
using Shared;

namespace WolverineTest.Worker.Features;

public class DailyForecast
{
    public static async Task<DailyForecastResponse> Handle(DailyForecastRequest request, ForecastIdentity identity)
    {
        var now = DateTimeOffset.Now;
        var stopwatch = Stopwatch.StartNew();

        // Simulate work
        await Task.Delay(request.Duration);

        stopwatch.Stop();
        return new DailyForecastResponse(
            request.Hour,
            identity.Id,
            identity.Name,
            now,
            stopwatch.Elapsed);
    }
}