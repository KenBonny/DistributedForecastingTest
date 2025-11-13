using JasperFx.Core;
using Shared;
using Wolverine;

namespace WolverineTest.Orchestrator.Features;

public class WeeklyForecastGeneration : Saga
{
    public Guid? Id { get; init; }

    public static (WeeklyForecastGeneration, IEnumerable<WeeklyForecastRequest>) Start(StartWeeklyForecast start)
    {
        var messages = Enum.GetValues<DayOfWeek>().Select(day => new WeeklyForecastRequest(day, 25.Seconds()));
        return (new WeeklyForecastGeneration { Id = Guid.CreateVersion7() }, messages);
    }
    
    public void Handle(WeeklyForecastResponse response)
    {
        
    }
}