using System.Text.RegularExpressions;

var inputLines = await File.ReadAllLinesAsync("input.txt");
var paths = inputLines
    .Select(line =>
        Regex.Match(line, @"(\w+)-(\w+)")
            .Groups.Values
            .Skip(1)
            .Select(group => group.Value)
            .ToArray()
    );

const string START = "start";
const string END = "end";

var allPaths = paths
    .Concat(paths.Select(path => path.Reverse().ToArray()))
    .Select(cave => (origin: cave[0], destination: cave[1]))
    .Where(path => path.destination != START)
    .Where(path => path.origin != END);

var routes = allPaths
    .GroupBy(k => k.origin, g => g.destination)
    .ToDictionary(k => k.Key, v => v.OrderBy(v => v).ToArray());

bool isSmallCave(string name) => char.IsLower(name[0]);

int walk(int paths, IList<string> path, string origin, Func<IEnumerable<string>, string, bool> selectDestination)
{
    if (origin == END)
        return paths + 1;

    path.Add(origin);

    var destinations = routes[origin]
        .Where(destination => selectDestination(path, destination))
        .ToArray();
    if (!destinations.Any())
        return paths;

    return destinations.AsParallel()
        .Aggregate(paths, (acc, destination) =>
            walk(acc, path.ToList(), destination, selectDestination)
        );
};

var result1 = walk(0, new List<string>(), START, (path, cave) =>
    !isSmallCave(cave) || !path.Contains(cave)
);

Console.WriteLine(result1);

{
    var smallCaves = routes
        .Select(route => route.Key)
        .Where(origin => origin != START)
        .Where(origin => isSmallCave(origin))
        .ToArray();

    var result2 = smallCaves.AsParallel()
        .Aggregate(0, (acc, caveToVisitTwice) =>
            walk(acc, new List<string>(), START, (route, cave) =>
                !isSmallCave(cave)
                || !route.Contains(cave)
                || (cave == caveToVisitTwice && route.Count(cave => cave == caveToVisitTwice) < 2)
            )
        );

    Console.WriteLine(result2 - (result1 * smallCaves.Length) + result1);
}
