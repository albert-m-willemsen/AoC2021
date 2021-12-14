using System.Collections.Concurrent;

var inputLines = await Runner.LoadInput("input.txt");

Runner.Run(inputLines, Day7Part1);
Runner.Run(inputLines, Day7Part2);

[Challenge(7, 1)]
static long Day7Part1(IImmutableList<string> lines)
{
    var positions = GetPositions(lines);

    var destinations = positions.Distinct()
        .OrderBy(v => v);

    return destinations.AsParallel()
        .Select(destination => CalculatTotalCost(positions, destination))
        .MinBy(t => t);
}

[Challenge(7, 2)]
static long Day7Part2(IImmutableList<string> lines)
{
    var positions = GetPositions(lines);

    var startDestintion = positions.Min();
    var endDestintation = positions.Max();
    var lookup = new ConcurrentDictionary<int, long>();

    return Enumerable.Range(startDestintion, endDestintation - startDestintion + 1).AsParallel()
        .Select(destination => CalculateStepsTotalCost(lookup, positions, destination))
        .MinBy(t => t);
}

static IImmutableList<int> GetPositions(IImmutableList<string> lines) =>
    lines[0].Split(',')
        .Select(v => Convert.ToInt32(v))
        .ToImmutableArray();

static long CalculateStep(int n) =>
    Enumerable.Range(1, n)
        .Aggregate<int, long>(0, (p, item) => p + item);

static int CalculatTotalCost(IImmutableList<int> positions, int destination) =>
    positions.Select(position => Math.Abs(destination - position))
        .Sum();

static long CalculateStepsTotalCost(ConcurrentDictionary<int, long> lookup, IImmutableList<int> positions, int destination) =>
    positions.Select(position => Math.Abs(destination - position))
        .AsParallel()
        .Select(step => lookup.GetOrAdd(step, n => CalculateStep(n)))
        .AsParallel()
        .AsEnumerable()
        .Sum();
