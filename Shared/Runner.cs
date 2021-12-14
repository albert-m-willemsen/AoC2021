using System.Collections.Immutable;
using System.Diagnostics;

namespace Shared
{
    public static class Runner
    {
        public static async Task<IImmutableList<string>> LoadInput(string filename) =>
            (await File.ReadAllLinesAsync(filename)).ToImmutableArray();

        public static void Run<T>(IImmutableList<string> lines, Func<IImmutableList<string>, T> function)
        {
            var sw = Stopwatch.StartNew();
            var result = function(lines);
            sw.Stop();

            var challenge = function.Method.GetCustomAttributes(typeof(ChallengeAttribute), false)
                .Cast<ChallengeAttribute>()
                .FirstOrDefault();

            Console.Write("Running challenge:");
            if (challenge is not null)
                Console.WriteLine($" Day {challenge.Day}, part {challenge.Part}.");
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine();
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ChallengeAttribute : Attribute
    {
        readonly int day;
        readonly int part;

        public ChallengeAttribute(int day, int part)
        {
            this.day = day;
            this.part = part;
        }

        public int Day => day;
        public int Part => part;
    }
}