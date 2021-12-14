const string START = "start";
const string END = "end";

var inputLines = await Runner.LoadInput("input.txt");

Runner.Run(inputLines, Day12Part1);
Runner.Run(inputLines, Day12Part2);

[Challenge(12, 1)]
static int Day12Part1(IImmutableList<string> lines) =>
    Calculate1(
        GetRoutes(
            GetPaths(lines)
        )
    );

[Challenge(12, 2)]
static int Day12Part2(IImmutableList<string> lines) =>
    Calculate2(
        GetRoutes(
            GetPaths(lines)
        )
    );

static string[][] GetPaths(IImmutableList<string> lines) => lines
    .Select(line => line.Split('-').ToArray())
    .ToArray();

static IDictionary<string, string[]> GetRoutes(string[][] paths) => paths
    .Concat(paths.Select(path => path.Reverse().ToArray()))
    .Select(cave => (origin: cave[0], destination: cave[1]))
    .Where(path => path.destination != START)
    .Where(path => path.origin != END)
    .GroupBy(k => k.origin, g => g.destination)
    .ToDictionary(k => k.Key, v => v.OrderBy(v => v).ToArray());

static int Calculate1(IDictionary<string, string[]> routes) => Walk(0, routes, new List<string>(), START, (path, cave) =>
    IsLargeCave(cave) || !path.Contains(cave)
);

static int Calculate2(IDictionary<string, string[]> routes)
{
    var smallCaves = routes
        .Select(route => route.Key)
        .Where(origin => origin != START)
        .Where(origin => !IsLargeCave(origin))
        .ToArray();

    var result1 = Calculate1(routes);

    var result2 = smallCaves.AsParallel()
        .Aggregate(0, (acc, caveToVisitTwice) =>
            Walk(acc, routes, new List<string>(), START, (route, cave) =>
                IsLargeCave(cave)
                || !route.Contains(cave)
                || (cave == caveToVisitTwice && route.Count(cave => cave == caveToVisitTwice) < 2)
            )
        );

    return result2 - (result1 * smallCaves.Length) + result1;
}

static bool IsLargeCave(string name) => char.IsUpper(name[0]);

static int Walk(int paths, IDictionary<string, string[]> routes, IList<string> path, string origin, Func<IEnumerable<string>, string, bool> selectDestination)
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
            Walk(acc, routes, path.ToList(), destination, selectDestination)
        );
};
