using JasperFx.Core;
using Shared;
using Wolverine;

namespace WolverineTest.Orchestrator.Features;

public class DailyForecastGeneration : Saga
{
    public Guid? Id { get; init; }

    public static (DailyForecastGeneration, IEnumerable<DailyForecastRequest>) Start(StartDailyForecast start)
    {
        var messages = Enumerable.Range(0, 24).Select(hour => new DailyForecastRequest(hour, 10.Seconds()));

        return (new DailyForecastGeneration { Id = Guid.CreateVersion7() }, messages);
    }
    
    public void Handle(DailyForecastResponse response)
    {
        
    }
}