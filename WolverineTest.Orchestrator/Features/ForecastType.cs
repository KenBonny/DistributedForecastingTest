namespace WolverineTest.Orchestrator.Features;

public enum ForecastType
{
    Daily = 0,
    Weekly = 1,
    Monthly = 2
}

public record StartForecast(ForecastType Type);