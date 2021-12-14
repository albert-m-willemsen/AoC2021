var inputLines = await Runner.LoadInput("input.txt");

Runner.Run(inputLines, Day13Part1);
Runner.Run(inputLines, Day13Part2);

[Challenge(13, 1)]
static long Day13Part1(IImmutableList<string> lines)
{
    var points = GetPoints(lines);
    var folds = GetFolds(lines);

    var paper = new Paper(points);

    var foldedPaper = folds.Take(1).Aggregate(paper, (acc, fold) => Fold(acc, fold));

    return foldedPaper.Points.Count;
}

[Challenge(13, 2)]
static string Day13Part2(IImmutableList<string> lines)
{
    var points = GetPoints(lines);
    var folds = GetFolds(lines);

    var paper = new Paper(points);

    var foldedPaper = folds.Aggregate(paper, (acc, fold) => Fold(acc, fold));

    PrintPaper(foldedPaper);

    return $"{foldedPaper.Width}x{foldedPaper.Height} -> {foldedPaper.Points.Count}";
};


static IImmutableSet<Point> GetPoints(IImmutableList<string> lines) =>
    lines.TakeWhile(l => !string.IsNullOrEmpty(l))
        .SelectMany(line =>
            line.Split(',')
                .Select(v => Convert.ToInt32(v))
                .Chunk(2)
                .Select(chunk => new Point(chunk[0], chunk[1]))
        )
        .ToImmutableHashSet();

static IImmutableList<Fold> GetFolds(IImmutableList<string> lines) =>
    lines.SkipWhile(l => !string.IsNullOrEmpty(l))
        .Skip(1)
        .SelectMany(l => l
            .Replace("fold along ", string.Empty)
            .Split('=')
            .Chunk(2)
            .Select(chunk => new Fold(Enum.Parse<Axis>(chunk[0]), Convert.ToInt32(chunk[1])))
        )
        .ToImmutableArray();

static Paper Fold(Paper paper, Fold fold) =>
    fold.Axis switch
    {
        Axis.x => FoldAroundX(paper, fold.Amount),
        Axis.y => FoldAroundY(paper, fold.Amount),
        _ => throw new NotImplementedException()
    };


static Paper FoldAroundX(Paper paper, int x) =>
    new(
        paper.Points
            .Where(p => p.X < x)
            .Select(p => new Point(p.X, p.Y))
            .UnionBy(paper.Points
                .Where(p => p.X > x)
                .Select(p => new Point(NewWidth(paper, x) - (p.X - x), p.Y)),
                p => p
            )
            .ToImmutableHashSet(),
        NewWidth(paper, x),
        paper.Height
    );

static int NewWidth(Paper paper, int x) =>
    paper.Width - x - 1;

static Paper FoldAroundY(Paper paper, int y) =>
    new(
        paper.Points
            .Where(p => p.Y < y)
            .Select(p => new Point(p.X, p.Y))
            .Union(paper.Points
                .Where(p => p.Y > y)
                .Select(p => new Point(p.X, NewHeight(paper, y) - (p.Y - y)))
            )
            .ToImmutableHashSet(),
       paper.Width,
       NewHeight(paper,y)
    );

static int NewHeight(Paper paper, int y) =>
    paper.Height - y - 1;

static void PrintPaper(Paper paper)
{
    var matrix = paper.Points.Aggregate(
        new ImmutableArray2D<bool>(false, paper.Width, paper.Height),
        (acc, p) => acc.SetItem(p.X, p.Y, true)
    );
    foreach (var row in Enumerable.Range(0, paper.Height))
    {
        foreach (var point in matrix.Row(row))
            if (point)
                Console.Write("#");
            else
                Console.Write(".");

        Console.WriteLine();
    }

    Console.WriteLine();
}

record Paper(IImmutableSet<Point> Points, int Width, int Height)
{
    public Paper(IImmutableSet<Point> points)
        : this(points, points.Max(p => p.X) + 1, points.Max(p => p.Y) + 1)
    { }
}

record Point(int X, int Y);

record Fold(Axis Axis, int Amount);

enum Axis { x, y }
