using System.Collections.Concurrent;

var inputLines = await File.ReadAllLinesAsync("input.txt");

var positions = inputLines[0].Split(',').Select(v => Convert.ToInt32(v)).ToArray();

{
    var destinations = positions.Distinct().OrderBy(v => v);
    var result = destinations.AsParallel()
        .Select(destination => (destination, total: CalculatTotalCost(positions, destination))).MinBy(t => t.total);

    Console.WriteLine(result);
}

{
    var startDestintion = positions.Min();
    var endDestintation = positions.Max();
    var lookup = new ConcurrentDictionary<int, long>();
    var result = Enumerable.Range(startDestintion, endDestintation - startDestintion + 1).AsParallel()
        .Select(destination => (destination, total: CalculateStepsTotalCost(lookup, positions, destination))).MinBy(t => t.total);

    Console.WriteLine(result);
}

static int CalculatTotalCost(int[] positions, int destination)
    => positions.Select(position => Math.Abs(destination - position)).Sum();

static long CalculateStepsTotalCost(ConcurrentDictionary<int, long> lookup, int[] positions, int destination)
    => positions.Select(position => Math.Abs(destination - position)).AsParallel().Select(step => lookup.GetOrAdd(step, n => CalculateStep(n))).AsParallel().AsEnumerable().Sum();

static long CalculateStep(int n)
    => Enumerable.Range(1, n).Aggregate<int, long>(0, (p, item) => p + item);