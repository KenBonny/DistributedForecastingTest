using JasperFx.Core;
using Shared;
using Wolverine;
using Wolverine.Marten;
using WolverineTest.Orchestrator.Domain;

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

    public static (DailyForecastGeneration, IEnumerable<DailyForecastRequest>, IStartStream) Start(StartDailyForecast start)
    {
        var id = Guid.CreateVersion7();
        var messages = Enumerable.Range(0, 24).Select(hour => new DailyForecastRequest(id, hour, 10.Seconds()));
        return (new DailyForecastGeneration { Id = id }, messages,
            MartenOps.StartStream<CalculationAggregate>(id, new Started("Daily")));
    }
    
    public IEnumerable<CalculationEvent> Handle(DailyForecastResponse response, [WriteAggregate] CalculationAggregate aggregate)
    {
        var events = new List<CalculationEvent>();
        if (_responses.Contains(
            response,
            EqualityComparer<DailyForecastResponse>.Create((left, right) => left?.Hour == right?.Hour)))
            return events;

        _responses.Add(response);
        events.Add(
            new PartDone(
                response.Hour.ToString(),
                response.ProcessorId,
                response.ProcessorName,
                response.StartTime,
                response.ActualDuration));
        
        if (Responses.Count == TotalHours)
        {
            events.Add(new Finished());
            MarkCompleted();
        }
        
        return events;
    }
}