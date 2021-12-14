var inputLines = await Runner.LoadInput("input.txt");

Runner.Run(inputLines, Day5Part1);
Runner.Run(inputLines, Day5Part2);

[Challenge(5, 1)]
static int Day5Part1(IImmutableList<string> lines)
{
    var lineQueue = ImmutableQueue.CreateRange(getLines(lines).Where(line => line.IsHorizontal || line.IsVertical));
    var intersectionSet = ImmutableHashSet<Point>.Empty;
    while (!lineQueue.IsEmpty)
    {
        lineQueue = lineQueue.Dequeue(out var comparer);

        foreach (var line in lineQueue)
            intersectionSet = intersectionSet.Union(comparer.Points.Intersect(line.Points));
    }

    return intersectionSet.Count;
}

[Challenge(5, 2)]
static int Day5Part2(IImmutableList<string> lines)
{
    var lineQueue = ImmutableQueue.CreateRange(getLines(lines));
    var intersectionSet = ImmutableHashSet<Point>.Empty;
    while (!lineQueue.IsEmpty)
    {
        lineQueue = lineQueue.Dequeue(out var comparer);

        foreach (var line in lineQueue)
            intersectionSet = intersectionSet.Union(comparer.Points.Intersect(line.Points));
    }

    return intersectionSet.Count;
}

static IImmutableList<Line> getLines(IImmutableList<string> lines) =>
    lines.SelectMany(line => line.Split(" -> ").SelectMany(coord => coord.Split(',')))
        .Select(v => Convert.ToInt32(v))
        .Chunk(2)
        .Select(v => new Point(v[0], v[1]))
        .Chunk(2)
        .Select(p => new Line(p[0], p[1]))
        .ToImmutableArray();

record Line
{
    readonly Lazy<IImmutableList<Point>> points;

    public Line(Point start, Point end)
    {
        Start = start;
        End = end;
        points = new(() => GeneratePoints().ToImmutableArray());
    }
    public Point Start { get; }
    public Point End { get; }

    public IImmutableList<Point> Points => points.Value;

    public bool IsHorizontal => Start.Y == End.Y;
    public bool IsVertical => Start.X == End.X;

    private IEnumerable<Point> GeneratePoints()
    {
        if (IsHorizontal)
            return GenerateHorizontalPoints();
        else if (IsVertical)
            return GenerateVerticalPoints();
        else
            return GenerateDiagonalPoints();
    }

    private IEnumerable<Point> GenerateVerticalPoints()
    {
        var minY = Math.Min(Start.Y, End.Y);
        var maxY = Math.Max(Start.Y, End.Y);
        return Enumerable
            .Range(minY, maxY - minY + 1)
            .Select(i => new Point(Start.X, i));
    }

    private IEnumerable<Point> GenerateHorizontalPoints()
    {
        var minX = Math.Min(Start.X, End.X);
        var maxX = Math.Max(Start.X, End.X);

        return Enumerable
            .Range(minX, maxX - minX + 1)
            .Select(i => new Point(i, Start.Y));
    }

    private IEnumerable<Point> GenerateDiagonalPoints()
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
            .Select(i => new Point(start.X + i, start.Y + i * direction));
    }
}

record Point(int X, int Y);
