var inputLines = await File.ReadAllLinesAsync("input.txt");

var heightMap = inputLines.Select(line => line.Select(c => c - '0').ToArray()).ToArray();

{
    var isLowPoint = (int[][] map, int x, int y) =>
    {
        var width = map[0].Length;
        var height = map.Length;

        var point = map[y][x];
        var left = x > 0 ? map[y][x - 1] : 10;
        var top = y > 0 ? map[y - 1][x] : 10;
        var right = x < width - 1 ? map[y][x + 1] : 10;
        var bottom = y < height - 1 ? map[y + 1][x] : 10;

        return point < Math.Min(Math.Min(left, right), Math.Min(top, bottom));
    };

    var lowPoints = heightMap.Select((line, y) => line.Where((_, x) => isLowPoint(heightMap, x, y)))
        .Aggregate((IEnumerable<int>)Array.Empty<int>(), (acc, line) => acc.Concat(line)).ToArray();
    var result = lowPoints.Select(point => point + 1).Sum();

    Console.WriteLine(result);
}

{
    var basinMap = heightMap.Select(line => line.Select(point => point < 9).ToArray()).ToArray();

    var walkBasin = (ref bool[][] map, int ix, int iy) =>
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

                map[p.y][p.x] = false;
                result++;
            }
        }

        return result;
    };

    var width = basinMap[0].Length;
    var height = basinMap.Length;
    var basins = new List<int>();
    for (var y = 0; y < height - 1; y++)
        for (var x = 0; x < width - 1; x++)
            if (basinMap[y][x])
                basins.Add(walkBasin(ref basinMap, x, y));

    var result = basins.OrderByDescending(x => x).Take(3).Aggregate(1, (acc, x) => acc * x);

    Console.WriteLine(result);
}
