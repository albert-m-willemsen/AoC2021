var inputLines = await Runner.LoadInput("input.txt");

Runner.Run(inputLines, Day9Part1);
Runner.Run(inputLines, Day9Part2);

[Challenge(9, 1)]
static int Day9Part1(IImmutableList<string> lines)
{
    var heightMap = getHeightMap(lines);
    return heightMap.Select((value, i) => isLowPoint(heightMap, i))
        .Aggregate(ImmutableArray<int>.Empty, (acc, line) => acc.Concat(line).ToImmutableArray())
        .Select(point => point + 1)
        .Sum();
}

[Challenge(9, 2)]
static int Day9Part2(IImmutableList<string> lines)
{
    var heightMap = getHeightMap(lines);
    var basinMap = new ImmutableArray2D<bool>(heightMap.Select(point => point < 9), heightMap.Width, heightMap.Height);

    var basinsSizes = new List<int>();
    for (var y = 0; y < basinMap.Height - 1; y++)
        for (var x = 0; x < basinMap.Width - 1; x++)
            if (basinMap[x, y])
                basinsSizes.Add(walkBasin(ref basinMap, x, y));

    return basinsSizes.OrderByDescending(x => x)
        .Take(3)
        .Aggregate(1, (acc, x) => acc * x);
}

static ImmutableArray2D<int> getHeightMap(IImmutableList<string> lines) =>
    new(
        lines.SelectMany(line =>
            line.Select(c => c - '0').ToImmutableArray()
        ).ToImmutableArray(),
        lines[0].Length,
        lines.Count
    );

static bool isLowPoint(ImmutableArray2D<int> map, int x, int y)
{
    var point = map[x, y];
    var left = x > 0 ? map[x - 1, y] : 10;
    var top = y > 0 ? map[x, y - 1] : 10;
    var right = x < map.Width - 1 ? map[x + 1, y] : 10;
    var bottom = y < map.Height - 1 ? map[x, y + 1] : 10;

    return point < Math.Min(Math.Min(left, right), Math.Min(top, bottom));
};

static int walkBasin(ref ImmutableArray<ImmutableArray<bool>> map, int ix, int iy)
{
    var result = 0;
    var queue = new HashSet<(int x, int y)> { (ix, iy) };

    var width = map[0].Length;
    var height = map.Length;

    while (queue.Any())
    {
        var p = queue.First();
        queue.Remove(p);

        if (map[p.y][p.x])
        {
            if (p.x > 0 && map[p.y][p.x - 1])
                queue.Add((p.x - 1, p.y));
            if (p.y > 0 && map[p.y - 1][p.x])
                queue.Add((p.x, p.y - 1));
            if (p.x < width - 1 && map[p.y][p.x + 1])
                queue.Add((p.x + 1, p.y));
            if (p.y < height - 1 && map[p.y + 1][p.x])
                queue.Add((p.x, p.y + 1));

            map.SetItem(p.y, map[p.y].SetItem(p.x, false));

            result++;
        }
    }

    return result;
};
