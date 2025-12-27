using JasperFx.Core;
using Shared;
using Wolverine;

namespace WolverineTest.Orchestrator.Features;

public class WeeklyForecastGeneration : Saga
{
    private static readonly int TotalDays = Enum.GetValues<DayOfWeek>().Length;
    private readonly List<WeeklyForecastResponse> _responses = new();

    public IReadOnlyCollection<WeeklyForecastResponse> Responses
    {
        get => _responses;
        init => _responses.AddRange(value);
    }

    public Guid Id { get; init; }

    public static (WeeklyForecastGeneration, IEnumerable<WeeklyForecastRequest>) Start(StartWeeklyForecast start)
    {
        var id = Guid.CreateVersion7();
        var messages = Enum.GetValues<DayOfWeek>().Select(day => new WeeklyForecastRequest(id, day, 25.Seconds()));
        return (new WeeklyForecastGeneration { Id = id }, messages);
    }
    
    public void Handle(WeeklyForecastResponse response)
    {
        if (_responses.Contains(
            response,
            EqualityComparer<WeeklyForecastResponse>.Create((left, right) => left?.Day == right?.Day)))
            return;

        _responses.Add(response);
        
        if (Responses.Count == TotalDays)
            MarkCompleted();
    }
}