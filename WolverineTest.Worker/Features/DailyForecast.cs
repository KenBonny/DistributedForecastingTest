using System.Diagnostics;
using Microsoft.Extensions.Options;
using Shared;

namespace WolverineTest.Worker.Features;

public class DailyForecastHandler
{
    public static async Task<DailyForecastResponse> Handle(
        DailyForecastRequest request,
        IOptions<WorkerIdentity> identity,
        IOptions<ForecastingEngine> forecastEngine)
    {
        var now = DateTimeOffset.Now;
        var stopwatch = Stopwatch.StartNew();

        // do work
        using var forecasting = Process.Start(forecastEngine.Value.Path.FullName, [request.Duration.ToString()]);
        await forecasting.WaitForExitAsync();

        stopwatch.Stop();
        return new DailyForecastResponse(
            request.Id,
            request.Hour,
            identity.Value.Id,
            identity.Value.Name,
            now,
            stopwatch.Elapsed);
    }
}