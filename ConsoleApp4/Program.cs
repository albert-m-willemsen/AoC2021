var inputLines = await Runner.LoadInput("input.txt");

Runner.Run(inputLines, Day4Part1);
Runner.Run(inputLines, Day4Part2);

[Challenge(4, 1)]
static int Day4Part1(IImmutableList<string> lines)
{
    var numbers = GetNumbers(lines);
    var boards = GetBoards(lines);

    foreach (var number in numbers)
        foreach (var board in boards)
            if (board.Mark(number))
                return board.SumUnmarked() * number;

    throw new InvalidOperationException();
}

[Challenge(4, 2)]
static int Day4Part2(IImmutableList<string> lines)
{
    var numbers = GetNumbers(lines);
    var boards = GetBoards(lines);

    foreach (var number in numbers)
        foreach (var board in boards)
        {
            board.Mark(number);
            if (boards.All(b => b.Won))
                return board.SumUnmarked() * number;
        }

    throw new InvalidOperationException();
}

static IImmutableList<int> GetNumbers(IImmutableList<string> lines) => lines[0]
    .Split(',')
    .Select(s => Convert.ToInt32(s))
    .ToImmutableArray();


static IImmutableList<Board> GetBoards(IImmutableList<string> lines) => lines
    .Skip(1)
    .Chunk(6)
    .Select(chunk => chunk
            .Skip(1)
            .SelectMany(v => v
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => Convert.ToInt32(v)
            )
        )
    )
    .Select(board => new Board(board, 5, 5))
    .ToImmutableArray();

class Board
{
    private ImmutableArray2D<bool> marks;
    private IImmutableDictionary<int, (int x, int y)> lookup;

    public Board(IEnumerable<int> board, int width, int height)
    {
        marks = new(false, width, height);
        lookup = board.Select((n, i) => (n, c: marks.ToCoords(i)))
            .ToImmutableDictionary(k => k.n, v => v.c);
    }

    public bool Won { get; private set; }

    public bool Mark(int number)
    {
        if (lookup.ContainsKey(number))
        {
            var (x, y) = lookup.GetValueOrDefault(number);
            lookup = lookup.Remove(number);

            marks = marks.SetItem(x, y, true);

            if (!Won)
                Won = marks.Row(y).All(m => m) || marks.Column(x).All(m => m);
        }

        return Won;
    }

    public int SumUnmarked() =>
        lookup.Keys.Sum();
}
