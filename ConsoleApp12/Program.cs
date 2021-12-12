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
    .Where(cave => cave[1] != START)
    .Where(cave => cave[0] != END);

var routing = allPaths
    .GroupBy(k => k[0], g => g[1])
    .OrderBy(route => route.Key, StringComparer.Ordinal)
    .ToDictionary(k => k.Key, v => v.OrderBy(v => v, StringComparer.Ordinal).ToArray());

bool isSmall(string name) => char.IsLower(name[0]);
bool isValid(IEnumerable<string> route) => route.LastOrDefault() == END;

int walk(int paths, IList<string> path, string origin, Func<IEnumerable<string>, string, bool> selectDestination)
{
    path.Add(origin);
    var getDestinations = () => routing[origin].Where(destination =>
        selectDestination(path, destination)
    );

    if (origin == END || !getDestinations().Any())
    {
        if (isValid(path))
            paths++;
        return paths;
    }

    return getDestinations().AsParallel()
        .Aggregate(paths, (acc, destination) =>
            walk(acc, path.ToList(), destination, selectDestination)
        );
};

{
    var result = walk(0, new List<string>(), START, (path, cave) =>
        !isSmall(cave) || !path.Contains(cave)
    );

    Console.WriteLine(result);
}

{
    var smallCaves = routing
        .Select(route => route.Key)
        .Where(origin => origin != START)
        .Where(origin => isSmall(origin))
        .ToArray();

    var normalResult = walk(0, new List<string>(), START, (path, cave) =>
        !isSmall(cave) || !path.Contains(cave)
    );

    var result = smallCaves.AsParallel()
        .Aggregate(0, (acc, caveToVisitTwice) =>
            walk(acc, new List<string>(), START, (route, cave) =>
                !isSmall(cave)
                || !route.Contains(cave)
                || (cave == caveToVisitTwice && route.Count(cave => cave == caveToVisitTwice) < 2)
            )
        );

    Console.WriteLine(result - (normalResult * smallCaves.Length) + normalResult);
}
