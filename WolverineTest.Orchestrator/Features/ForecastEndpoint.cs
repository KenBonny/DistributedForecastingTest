using Wolverine.Http;

namespace WolverineTest.Orchestrator.Features;

public class ForecastEndpoint
{
    [WolverinePost("/forecast/random")]
    public static IResult Random(int? numberOfForecasts)
    {
        return Results.Ok(numberOfForecasts ?? 10);
    }
}

