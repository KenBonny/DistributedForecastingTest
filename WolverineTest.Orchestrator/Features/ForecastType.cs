namespace WolverineTest.Orchestrator.Features;

public record StartForecast;

public record StartDailyForecast : StartForecast;

public record StartWeeklyForecast : StartForecast;

public record StartMonthlyForecast : StartForecast;
