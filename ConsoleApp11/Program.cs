var inputLines = await File.ReadAllLinesAsync("input.txt");

/// <summary>
/// Day 11, part 1.
/// </summary>
Performance.Measure(() =>
{
    var data = generateData(inputLines);

    return Enumerable.Range(1, 100)
        .Aggregate(0, (acc, _) => acc + simulate(ref data));
});

/// <summary>
/// Day 11, part 2.
/// </summary>
Performance.Measure(() =>
{
    var step = 0;
    var data = generateData(inputLines);

    while (!allFlashed(ref data))
    {
        simulate(ref data);
        step++;
    }

    return step;
});

static (int v, bool f)[][] generateData(string[] lines) => lines
    .Select(line => line.Select(c => c - '0').ToArray())
    .Select(row => row.Select(col => (col, false)).ToArray())
    .ToArray();

static void clearFlashed(ref (int v, bool)[][] data)
{
    var width = data[0].Length;
    var height = data.Length;
    for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            data[y][x] = (data[y][x].v, false);
}

static int simulate(ref (int v, bool f)[][] data)
{
    clearFlashed(ref data);

    int flashes = 0;
    var width = data[0].Length;
    var height = data.Length;

    for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            data[y][x].v += 1;

    for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            flashes += flash(ref data, x, y);

    for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            if (data[y][x].v > 9)
                data[y][x].v = 0;

    return flashes;
};

static int flash(ref (int v, bool f)[][] data, int x, int y)
{
    if (data[y][x].f || data[y][x].v <= 9)
        return 0;
    data[y][x].f = true;

    var flashes = 1;
    var width = data[0].Length;
    var height = data.Length;
    var hasLeft = x > 0;
    var hasRight = x < width - 1;
    var hasUp = y > 0;
    var hasDown = y < height - 1;

    if (hasLeft)
        data[y][x - 1].v += 1;
    if (hasUp)
        data[y - 1][x].v += 1;
    if (hasRight)
        data[y][x + 1].v += 1;
    if (hasDown)
        data[y + 1][x].v += 1;
    if (hasLeft && hasUp)
        data[y - 1][x - 1].v += 1;
    if (hasRight && hasUp)
        data[y - 1][x + 1].v += 1;
    if (hasLeft && hasDown)
        data[y + 1][x - 1].v += 1;
    if (hasRight && hasDown)
        data[y + 1][x + 1].v += 1;

    if (hasLeft)
        flashes += flash(ref data, x - 1, y);
    if (hasUp)
        flashes += flash(ref data, x, y - 1);
    if (hasRight)
        flashes += flash(ref data, x + 1, y);
    if (hasDown)
        flashes += flash(ref data, x, y + 1);
    if (hasLeft && hasUp)
        flashes += flash(ref data, x - 1, y - 1);
    if (hasRight && hasUp)
        flashes += flash(ref data, x + 1, y - 1);
    if (hasLeft && hasDown)
        flashes += flash(ref data, x - 1, y + 1);
    if (hasRight && hasDown)
        flashes += flash(ref data, x + 1, y + 1);

    return flashes;
}

static bool allFlashed(ref (int, bool f)[][] data) =>
    data.All(row => row.All(col => col.f));
