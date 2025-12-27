using JasperFx.Core;
using Shared;
using Wolverine;

namespace WolverineTest.Orchestrator.Features;

public class DailyForecastGeneration : Saga
{
    private const int TotalHours = 24;
    public Guid Id { get; init; }
    private readonly List<DailyForecastResponse> _responses = new();

    public IReadOnlyCollection<DailyForecastResponse> Responses
    {
        get => _responses;
        init => _responses.AddRange(value);
    }

    public static (DailyForecastGeneration, IEnumerable<DailyForecastRequest>) Start(StartDailyForecast start)
    {
        var id = Guid.CreateVersion7();
        var messages = Enumerable.Range(0, 24).Select(hour => new DailyForecastRequest(id, hour, 10.Seconds()));
        return (new DailyForecastGeneration { Id = id }, messages);
    }
    
    public void Handle(DailyForecastResponse response)
    {
        if (_responses.Contains(
            response,
            EqualityComparer<DailyForecastResponse>.Create((left, right) => left?.Hour == right?.Hour)))
            return;

        _responses.Add(response);
        
        if (Responses.Count == TotalHours)
            MarkCompleted();
    }
}