using System.Collections.Concurrent;

var inputLines = await File.ReadAllLinesAsync("input.txt");

var positions = inputLines[0].Split(',').Select(v => Convert.ToInt32(v)).ToArray();

var calculateStep = (int n) =>
    Enumerable.Range(1, n).Aggregate<int, long>(0, (p, item) => p + item);

var calculatTotalCost = (int[] positions, int destination) =>
    positions.Select(position => Math.Abs(destination - position)).Sum();

var calculateStepsTotalCost = (ConcurrentDictionary<int, long> lookup, int[] positions, int destination) =>
    positions.Select(position => Math.Abs(destination - position))
        .AsParallel()
        .Select(step => lookup.GetOrAdd(step, n => calculateStep(n)))
        .AsParallel()
        .AsEnumerable()
        .Sum();

{
    var destinations = positions.Distinct().OrderBy(v => v);
    var result = destinations.AsParallel()
        .Select(destination => (destination, total: calculatTotalCost(positions, destination)))
        .MinBy(t => t.total);

    Console.WriteLine(result);
}

{
    var startDestintion = positions.Min();
    var endDestintation = positions.Max();
    var lookup = new ConcurrentDictionary<int, long>();
    var result = Enumerable.Range(startDestintion, endDestintation - startDestintion + 1).AsParallel()
        .Select(destination => (destination, total: calculateStepsTotalCost(lookup, positions, destination)))
        .MinBy(t => t.total);

    Console.WriteLine(result);
}
