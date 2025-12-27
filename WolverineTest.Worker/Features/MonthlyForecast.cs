using System.Diagnostics;
using Shared;

namespace WolverineTest.Worker.Features;

public class MonthlyForecastHandler
{
    public static async Task<MonthlyForecastResponse> Handle(MonthlyForecastRequest request, ForecastIdentity identity)
    {
        var now = DateTimeOffset.Now;
        var stopwatch = Stopwatch.StartNew();

        // Simulate work
        await Task.Delay(request.Duration);

        stopwatch.Stop();
        return new MonthlyForecastResponse(
            request.Day,
            identity.Id,
            identity.Name,
            now,
            stopwatch.Elapsed);
    }
}