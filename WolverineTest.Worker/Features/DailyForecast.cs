using System.Diagnostics;
using Microsoft.Extensions.Options;
using Shared;

namespace WolverineTest.Worker.Features;

public class DailyForecastHandler
{
    public static async Task<DailyForecastResponse> Handle(
        DailyForecastRequest request,
        IOptions<WorkerIdentity> identity,
        IOptions<FileInfo> forecastEngine)
    {
        var now = DateTimeOffset.Now;
        var stopwatch = Stopwatch.StartNew();

        // Simulate work
        await Task.Delay(request.Duration);

        stopwatch.Stop();
        return new DailyForecastResponse(
            request.Hour,
            identity.Value.Id,
            identity.Value.Name,
            now,
            stopwatch.Elapsed);
    }
}