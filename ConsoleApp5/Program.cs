var inputLines = await File.ReadAllLinesAsync("input.txt");

/// <summary>
/// Day 5, part 1.
/// </summary>
Performance.Measure(() =>
{
    var lines = getLines(inputLines);

    var lineSet = new Queue<Line>(lines.Where(line => line.IsHorizontal || line.IsVertical));
    var intersections = new HashSet<Point>();
    while (lineSet.Any())
    {
        var comparer = lineSet.Dequeue();

        foreach (var line in lineSet.ToArray())
            intersections.UnionWith(comparer.Points.Intersect(line.Points));
    }

    return intersections.Count;
});

/// <summary>
/// Day 5, part 2.
/// </summary>
Performance.Measure(() =>
{
    var lines = getLines(inputLines);

    var lineSet = new Queue<Line>(lines);
    var intersections = new HashSet<Point>();
    while (lineSet.Any())
    {
        var comparer = lineSet.Dequeue();

        foreach (var line in lineSet.ToArray())
            intersections.UnionWith(comparer.Points.Intersect(line.Points));
    }

    return intersections.Count;
});

static Line[] getLines(string[] lines) =>
    lines.Select(line => line.Split(" -> ").SelectMany(coord => coord.Split(',')))
    .Select(g => g.Select(v => Convert.ToInt32(v)).ToArray())
    .Select(v => new Line(new Point(v[0], v[1]), new Point(v[2], v[3])))
    .ToArray();

record Line
{
    readonly Lazy<Point[]> points;

    public Line(Point start, Point end)
    {
        Start = start;
        End = end;
        points = new(() => GeneratePoints());
    }
    public Point Start { get; }
    public Point End { get; }

    public Point[] Points => points.Value;

    public bool IsHorizontal => Start.Y == End.Y;
    public bool IsVertical => Start.X == End.X;

    private Point[] GeneratePoints()
    {
        if (IsHorizontal)
            return GenerateHorizontalPoints();
        else if (IsVertical)
            return GenerateVerticalPoints();
        else
            return GenerateDiagonalPoints();
    }

    private Point[] GenerateVerticalPoints()
    {
        var minY = Math.Min(Start.Y, End.Y);
        var maxY = Math.Max(Start.Y, End.Y);
        return Enumerable
            .Range(minY, maxY - minY + 1)
            .Select(i => new Point(Start.X, i))
            .ToArray();
    }

    private Point[] GenerateHorizontalPoints()
    {
        var minX = Math.Min(Start.X, End.X);
        var maxX = Math.Max(Start.X, End.X);

        return Enumerable
            .Range(minX, maxX - minX + 1)
            .Select(i => new Point(i, Start.Y))
            .ToArray();
    }

    private Point[] GenerateDiagonalPoints()
    {
        var minX = Math.Min(Start.X, End.X);
        var maxX = Math.Max(Start.X, End.X);

        var start = Start;
        var end = End;
        var direction = 1;

        var isLeft = Start.X - End.X < 0;
        if (!isLeft)
        {
            start = End;
            end = Start;
        }
        var isAbove = start.Y - end.Y < 0;
        if (!isAbove)
            direction = -1;

        return Enumerable
            .Range(0, maxX - minX + 1)
            .Select(i => new Point(start.X + i, start.Y + i * direction))
            .ToArray();
    }
}

record Point(int X, int Y);
