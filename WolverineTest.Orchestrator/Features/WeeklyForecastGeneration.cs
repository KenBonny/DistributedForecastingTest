using JasperFx.Core;
using Shared;
using Wolverine;

namespace WolverineTest.Orchestrator.Features;

public class WeeklyForecastGeneration : Saga
{
    public Guid? Id { get; init; }

    public static (WeeklyForecastGeneration, IEnumerable<WeeklyForecastRequest>) Start(StartWeeklyForecast start)
    {
        var id = Guid.CreateVersion7();
        var messages = Enum.GetValues<DayOfWeek>().Select(day => new WeeklyForecastRequest(id, day, 25.Seconds()));
        return (new WeeklyForecastGeneration { Id = id }, messages);
    }
    
    public void Handle(WeeklyForecastResponse response)
    {
        
    }
}