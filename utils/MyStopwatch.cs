using System.Diagnostics;

namespace utils;

public class MyStopwatch : IDisposable
{
    private readonly Stopwatch _stopwatch;

    public MyStopwatch()
    {
        _stopwatch = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        _stopwatch.Stop();
        Console.WriteLine($"Time spent: {_stopwatch.ElapsedMilliseconds} ms");
    }
}