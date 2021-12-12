using System.Diagnostics;

static class Performance
{
    public static void Measure<T>(Func<T> function)
    {
        var sw = Stopwatch.StartNew();
        var result = function();
        sw.Stop();

        Console.WriteLine($"{result}: {sw.ElapsedMilliseconds}ms");
    }
}
