using JasperFx.Core;
using Shared;
using Wolverine;
using Wolverine.Marten;
using WolverineTest.Orchestrator.Domain;

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

    public static (MonthlyForecastGeneration, IEnumerable<MonthlyForecastRequest>, IStartStream) Start(StartMonthlyForecast start)
    {
        var id = Guid.CreateVersion7();
        var messages = Enumerable.Range(1, 30).Select(day => new MonthlyForecastRequest(id, day, 60.Seconds()));
        return (new MonthlyForecastGeneration { Id = id }, messages,
            MartenOps.StartStream<CalculationAggregate>(id, new Started("Monthly")));
    }
    
    public IEnumerable<CalculationEvent> Handle(MonthlyForecastResponse response, [WriteAggregate] CalculationAggregate aggregate)
    {
        var events = new List<CalculationEvent>();
        if (_responses.Contains(
            response,
            EqualityComparer<MonthlyForecastResponse>.Create((left, right) => left?.Day == right?.Day)))
            return events;

        _responses.Add(response);
        events.Add(
            new PartDone(
                response.Day.ToString(),
                response.ProcessorId,
                response.ProcessorName,
                response.StartTime,
                response.ActualDuration));
        
        if (Responses.Count == TotalDays)
        {
            events.Add(new Finished());
            MarkCompleted();
        }
        
        return events;
    }
}