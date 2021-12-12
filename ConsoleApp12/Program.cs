var inputLines = await File.ReadAllLinesAsync("input.txt");

const string START = "start";
const string END = "end";

/// <summary>
/// Day 12, part 1.
/// </summary>
Performance.Measure(() =>
{
    var paths = getPaths(inputLines);
    var routes = getRoutes(paths);

    return calculate1(routes);
});

/// <summary>
/// Day 12, part 2.
/// </summary>
Performance.Measure(() =>
{
    var paths = getPaths(inputLines);
    var routes = getRoutes(paths);

    return calculate2(routes);
});

static int calculate1(IDictionary<string, string[]> routes) => walk(0, routes, new List<string>(), START, (path, cave) =>
    !isSmallCave(cave) || !path.Contains(cave)
);

static int calculate2(IDictionary<string, string[]> routes)
{
    var smallCaves = routes
        .Select(route => route.Key)
        .Where(origin => origin != START)
        .Where(origin => isSmallCave(origin))
        .ToArray();

    var result1 = calculate1(routes);

    var result2 = smallCaves.AsParallel()
        .Aggregate(0, (acc, caveToVisitTwice) =>
            walk(acc, routes, new List<string>(), START, (route, cave) =>
                !isSmallCave(cave)
                || !route.Contains(cave)
                || (cave == caveToVisitTwice && route.Count(cave => cave == caveToVisitTwice) < 2)
            )
        );

    return result2 - (result1 * smallCaves.Length) + result1;
}

static string[][] getPaths(string[] lines) => lines
    .Select(line => line.Split('-').ToArray())
    .ToArray();

static IDictionary<string, string[]> getRoutes(string[][] paths) => paths
    .Concat(paths.Select(path => path.Reverse().ToArray()))
    .Select(cave => (origin: cave[0], destination: cave[1]))
    .Where(path => path.destination != START)
    .Where(path => path.origin != END)
    .GroupBy(k => k.origin, g => g.destination)
    .ToDictionary(k => k.Key, v => v.OrderBy(v => v).ToArray());

static bool isSmallCave(string name) => char.IsLower(name[0]);

static int walk(int paths, IDictionary<string, string[]> routes, IList<string> path, string origin, Func<IEnumerable<string>, string, bool> selectDestination)
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
            walk(acc, routes, path.ToList(), destination, selectDestination)
        );
};
