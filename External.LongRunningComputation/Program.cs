
var durationParseSuccess = TimeSpan.TryParse(args[0], out var duration);

if (!durationParseSuccess)
{
    Console.WriteLine("Invalid duration format. Please provide a valid time span string.");
    Environment.ExitCode = -1;
    return;
}

Thread.Sleep(duration);

switch (Random.Shared.Next(100))
{
    case < 10:
        Console.WriteLine("Unrecoverable error detected");
        Environment.ExitCode = -2;
        return;
    case < 20:
        Console.WriteLine("Recoverable error detected");
        Environment.ExitCode = -3;
        return;
    default:
        Console.WriteLine($"Operation completed successfully after {duration}");
        Environment.ExitCode = 0;
        return;
}