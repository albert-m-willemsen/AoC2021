using System.Collections.Concurrent;

var inputLines = await File.ReadAllLinesAsync("input.txt");

/// <summary>
/// Day 7, part 1.
/// </summary>
Performance.Measure(() =>
{
    var positions = getPositions(inputLines);

    var destinations = positions.Distinct()
        .OrderBy(v => v);

    return destinations.AsParallel()
        .Select(destination => (destination, total: calculatTotalCost(positions, destination)))
        .MinBy(t => t.total);
});

/// <summary>
/// Day 7, part 2.
/// </summary>
Performance.Measure(() =>
{
    var positions = getPositions(inputLines);

    var startDestintion = positions.Min();
    var endDestintation = positions.Max();
    var lookup = new ConcurrentDictionary<int, long>();

    return Enumerable.Range(startDestintion, endDestintation - startDestintion + 1).AsParallel()
        .Select(destination => (destination, total: calculateStepsTotalCost(lookup, positions, destination)))
        .MinBy(t => t.total);
});

static int[] getPositions(string[] lines) => lines[0].Split(',')
    .Select(v => Convert.ToInt32(v))
    .ToArray();

static long calculateStep(int n) => Enumerable.Range(1, n)
    .Aggregate<int, long>(0, (p, item) => p + item);

static int calculatTotalCost(int[] positions, int destination) =>
    positions.Select(position => Math.Abs(destination - position))
        .Sum();

long calculateStepsTotalCost(ConcurrentDictionary<int, long> lookup, int[] positions, int destination) =>
    positions.Select(position => Math.Abs(destination - position))
        .AsParallel()
        .Select(step => lookup.GetOrAdd(step, n => calculateStep(n)))
        .AsParallel()
        .AsEnumerable()
        .Sum();
