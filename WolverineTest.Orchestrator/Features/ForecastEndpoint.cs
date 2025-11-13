using Wolverine;
using Wolverine.Http;

namespace WolverineTest.Orchestrator.Features;

public class ForecastEndpoint
{
    [WolverinePost("/forecast/random")]
    public static (IResult, OutgoingMessages) Random(int? numberOfForecasts)
    {
        var amount = numberOfForecasts ?? 10;
        var startForecasts = Enumerable.Range(0, amount)
            .Select(i => GetForecastType().DelayedFor(TimeSpan.FromSeconds(i * 5)))
            .ToList();
        return (Results.Ok(startForecasts), new OutgoingMessages(startForecasts));
    }

    private static StartForecast GetForecastType() =>
        System.Random.Shared.Next(100) switch
        {
            < 60 => new StartDailyForecast(),
            < 90 => new StartWeeklyForecast(),
            _ => new StartMonthlyForecast()
        };
    
    [WolverinePost("/forecast/deterministic")]
    public static (IResult, OutgoingMessages) Deterministic(int? numberOfForecasts)
    {
        var amount = numberOfForecasts ?? 10;
        var startForecasts = ForecastTypes()
            .Take(amount)
            .Select((type, i) => type.DelayedFor(TimeSpan.FromSeconds(i * 5)))
            .ToList();
        return (Results.Ok(startForecasts), new OutgoingMessages(startForecasts));
    }

    private static IEnumerable<StartForecast> ForecastTypes()
    {
        while (true)
        {
            yield return new StartWeeklyForecast();
            yield return new StartDailyForecast();
            yield return new StartWeeklyForecast();
            yield return new StartDailyForecast();
            yield return new StartDailyForecast();
            yield return new StartDailyForecast();
            yield return new StartMonthlyForecast();
            yield return new StartDailyForecast();
            yield return new StartWeeklyForecast();
            yield return new StartDailyForecast();
        }
    }
}

