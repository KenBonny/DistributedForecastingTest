namespace Shared;

public record DailyForecastRequest(int Hour, TimeSpan Duration);

public record DailyForecastResponse(
    int Hour,
    int ProcessorId,
    string? ProcessorName,
    DateTimeOffset StartTime,
    TimeSpan ActualDuration
);

public record WeeklyForecastRequest(DayOfWeek Day, TimeSpan Duration);

public record WeeklyForecastResponse(
    DayOfWeek Day,
    int ProcessorId,
    string? ProcessorName,
    DateTimeOffset StartTime,
    TimeSpan ActualDuration
);

public record MonthlyForecastRequest(int Day, TimeSpan Duration);

public record MonthlyForecastResponse(
    int Day,
    int ProcessorId,
    string? ProcessorName,
    DateTimeOffset StartTime,
    TimeSpan ActualDuration
);