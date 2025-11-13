using Wolverine.Http;

namespace WolverineTest.Orchestrator.Features;

public class ForecastEndpoint
{
    [WolverinePost("/forecast/random")]
    public static (IResult, IEnumerable<StartForecast>) Random(int? numberOfForecasts)
    {
        var amount = numberOfForecasts ?? 10;
        var startForecasts = Enumerable.Range(0, amount)
            .Select(_ => GetForecastType())
            .Select(type => new StartForecast(type))
            .ToList();
        return (Results.Ok(startForecasts), startForecasts);
    }

    private static ForecastType GetForecastType() =>
        System.Random.Shared.Next(100) switch
        {
            < 60 => ForecastType.Daily,
            < 90 => ForecastType.Weekly,
            _ => ForecastType.Monthly
        };
    
    [WolverinePost("/forecast/deterministic")]
    public static (IResult, IEnumerable<StartForecast>) Deterministic(int? numberOfForecasts)
    {
        var amount = numberOfForecasts ?? 10;
        var startForecasts = ForecastTypes().Take(amount).Select(type => new StartForecast(type)).ToList();
        return (Results.Ok(startForecasts), startForecasts);
    }

    private static IEnumerable<ForecastType> ForecastTypes()
    {
        while (true)
        {
            yield return ForecastType.Weekly;
            yield return ForecastType.Daily;
            yield return ForecastType.Weekly;
            yield return ForecastType.Daily;
            yield return ForecastType.Daily;
            yield return ForecastType.Daily;
            yield return ForecastType.Monthly;
            yield return ForecastType.Daily;
            yield return ForecastType.Weekly;
            yield return ForecastType.Daily;
        }
    }
}

