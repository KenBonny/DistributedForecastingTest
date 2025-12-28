using JasperFx.Core;
using Shared;
using Wolverine;
using Wolverine.Marten;
using WolverineTest.Orchestrator.Domain;

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

    public static (WeeklyForecastGeneration, IEnumerable<WeeklyForecastRequest> messages,
        IStartStream) Start(StartWeeklyForecast start)
    {
        var id = Guid.CreateVersion7();
        var messages = Enum.GetValues<DayOfWeek>().Select(day => new WeeklyForecastRequest(id, day, 25.Seconds()));
        return (new WeeklyForecastGeneration { Id = id }, messages,
            MartenOps.StartStream<CalculationAggregate>(id, new Started("Weekly")));
    }

    public IEnumerable<CalculationEvent> Handle(
        WeeklyForecastResponse response,
        [WriteAggregate] CalculationAggregate aggregate)
    {
        var events = new List<CalculationEvent>();
        if (_responses.Contains(
            response,
            EqualityComparer<WeeklyForecastResponse>.Create((left, right) => left?.Day == right?.Day)))
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