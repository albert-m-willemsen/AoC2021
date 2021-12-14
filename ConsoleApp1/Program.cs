var inputLines = await Runner.LoadInput("input.txt");

Runner.Run(inputLines, Day1Part1);
Runner.Run(inputLines, Day1Part2);

[Challenge(1, 1)]
static int Day1Part1(IImmutableList<string> lines) =>
    CountIncreases(
        GetNumbers(lines)
    );

[Challenge(1, 2)]
static int Day1Part2(IImmutableList<string> lines) =>
    CountIncreases(
        CreateWindow(
                GetNumbers(lines),
                3,
                Add
            )
    );

static IImmutableList<int> GetNumbers(IImmutableList<string> lines) =>
    lines.Select(line => Convert.ToInt32(line))
        .ToImmutableArray();

static int CountIncreases(IEnumerable<int> numbers) =>
    numbers.Skip(1)
        .Zip(numbers, IsIncreased)
        .Count(w => w);

static bool IsIncreased(int curr, int prev) =>
    curr > prev;

static IImmutableList<int> CreateWindow(IEnumerable<int> numbers, int size, Func<int, int, int> func) =>
    Enumerable.Range(1, size - 1)
        .Aggregate(numbers, (acc, s) => acc.Zip(numbers.Skip(s), func))
        .ToImmutableArray();

static int Add(int l, int r) => l + r;
