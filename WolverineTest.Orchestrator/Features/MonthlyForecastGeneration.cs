using JasperFx.Core;
using Shared;
using Wolverine;

namespace WolverineTest.Orchestrator.Features;

public class MonthlyForecastGeneration : Saga
{
    public Guid? Id { get; init; }

    public static (MonthlyForecastGeneration, IEnumerable<MonthlyForecastRequest>) Start(StartMonthlyForecast start)
    {
        var id = Guid.CreateVersion7();
        var messages = Enumerable.Range(1, 30).Select(day => new MonthlyForecastRequest(id, day, 60.Seconds()));
        return (new MonthlyForecastGeneration { Id = id }, messages);
    }
    
    public void Handle(MonthlyForecastResponse response)
    {
        
    }
}