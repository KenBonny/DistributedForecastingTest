namespace WolverineTest.Orchestrator.Domain;

public record CalculationAggregate
{
    public Guid Id { get; init; }
    public required string Type { get; init; }
    private readonly List<Calculation> _calculations = new();
    public IReadOnlyCollection<Calculation> Calculations => _calculations;
    public Status Status { get; init; } = Status.Started;
    
    public void Add(Calculation calculation) => _calculations.Add(calculation);

    public static CalculationAggregate Create(Started started) =>
        new() { Type = started.Type };

    public static CalculationAggregate Handle(CalculationAggregate current, PartDone evnt)
    {
        current.Add(
            new Calculation(
                evnt.Data,
                evnt.ProcessorId,
                evnt.ProcessorName,
                evnt.StartTime,
                evnt.ActualDuration));
        
        return current;
    }

    public static CalculationAggregate Handle(CalculationAggregate current, Failed evnt)
    {
        current.Add(
            new Calculation(
                evnt.Data,
                evnt.ProcessorId,
                evnt.ProcessorName,
                evnt.StartTime,
                evnt.ActualDuration));

        return current with { Status = Status.Failed };
    }
    
    public static CalculationAggregate Handle(CalculationAggregate current, Retried evnt) => current;

    public static CalculationAggregate Handle(CalculationAggregate current, Finished evnt) =>
        current with { Status = Status.Finished };
}

public record Calculation(
    string Data,
    int ProcessorId,
    string? ProcessorName,
    DateTimeOffset StartTime,
    TimeSpan ActualDuration
);

public enum Status
{
    Started,
    Finished,
    Failed
}

public abstract record CalculationEvent;

public record Started(string Type) : CalculationEvent;

public record PartDone(
    string Data,
    int ProcessorId,
    string? ProcessorName,
    DateTimeOffset StartTime,
    TimeSpan ActualDuration
) : CalculationEvent;

public record Failed(
    string Data,
    string Reason,
    int ProcessorId,
    string? ProcessorName,
    DateTimeOffset StartTime,
    TimeSpan ActualDuration
) : CalculationEvent;

public record Retried(
    int ProcessorId,
    string? ProcessorName,
    DateTimeOffset StartTime,
    TimeSpan ActualDuration
) : CalculationEvent;

public record Finished() : CalculationEvent;