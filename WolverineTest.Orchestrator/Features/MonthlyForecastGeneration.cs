using JasperFx.Core;
using Shared;
using Wolverine;

namespace WolverineTest.Orchestrator.Features;

public class MonthlyForecastGeneration : Saga
{
    private const int TotalDays = 30;
    public Guid Id { get; init; }
    private readonly List<MonthlyForecastResponse> _responses = new();

    public IReadOnlyCollection<MonthlyForecastResponse> Responses
    {
        get => _responses;
        init => _responses.AddRange(value);
    }

    public static (MonthlyForecastGeneration, IEnumerable<MonthlyForecastRequest>) Start(StartMonthlyForecast start)
    {
        var id = Guid.CreateVersion7();
        var messages = Enumerable.Range(1, 30).Select(day => new MonthlyForecastRequest(id, day, 60.Seconds()));
        return (new MonthlyForecastGeneration { Id = id }, messages);
    }
    
    public void Handle(MonthlyForecastResponse response)
    {
        if (_responses.Contains(
            response,
            EqualityComparer<MonthlyForecastResponse>.Create((left, right) => left?.Day == right?.Day)))
            return;

        _responses.Add(response);
        
        if (Responses.Count == TotalDays)
            MarkCompleted();
    }
}