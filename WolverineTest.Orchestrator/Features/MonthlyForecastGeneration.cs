using JasperFx.Core;
using Shared;
using Wolverine;

namespace WolverineTest.Orchestrator.Features;

public class MonthlyForecastGeneration : Saga
{
    public Guid? Id { get; init; }

    public static (MonthlyForecastGeneration, IEnumerable<MonthlyForecastRequest>) Start(StartMonthlyForecast start)
    {
        var messages = Enumerable.Range(1, 30).Select(day => new MonthlyForecastRequest(day, 60.Seconds()));
        return (new MonthlyForecastGeneration { Id = Guid.CreateVersion7() }, messages);
    }
    
    public void Handle(MonthlyForecastResponse response)
    {
        
    }
}