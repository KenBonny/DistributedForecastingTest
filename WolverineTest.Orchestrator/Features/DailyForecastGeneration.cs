using JasperFx.Core;
using Shared;
using Wolverine;

namespace WolverineTest.Orchestrator.Features;

public class DailyForecastGeneration : Saga
{
    public Guid Id { get; init; }

    public static (DailyForecastGeneration, IEnumerable<DailyForecastRequest>) Start(StartDailyForecast start)
    {
        var id = Guid.CreateVersion7();
        var messages = Enumerable.Range(0, 24).Select(hour => new DailyForecastRequest(id, hour, 10.Seconds()));
        return (new DailyForecastGeneration { Id = id }, messages);
    }
    
    public void Handle(DailyForecastResponse response)
    {
        
    }
}