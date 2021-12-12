var inputLines = await File.ReadAllLinesAsync("input.txt");

var input = inputLines.Select(line => line.Select(c => c - '0').ToArray()).ToArray();

(int v, bool f)[][] generateData() => input.Select(row => row.Select(col => (col, false)).ToArray()).ToArray();

void clearFlashed(ref (int v, bool)[][] data)
{
    var width = data[0].Length;
    var height = data.Length;
    for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            data[y][x] = (data[y][x].v, false);
}

int simulate(ref (int v, bool f)[][] data)
{
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

int flash(ref (int v, bool f)[][] data, int x, int y)
{
    if (data[y][x].f || data[y][x].v <= 9)
        return 0;
    data[y][x].f = true;

    var flashes = 1;
    var width = input[0].Length;
    var height = input.Length;
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

{
    var totalFlashes = 0;
    var data = generateData();

    for (var s = 1; s <= 100; s++)
    {
        clearFlashed(ref data);
        totalFlashes += simulate(ref data);
    }

    Console.WriteLine(totalFlashes);
}

bool allFlashed(ref (int, bool f)[][] data)
{
    return data.All(row => row.All(col => col.f));
}

{
    var step = 0;
    var data = generateData();

    while (!allFlashed(ref data))
    {
        clearFlashed(ref data);
        simulate(ref data);
        step++;
    }

    Console.WriteLine(step);
}