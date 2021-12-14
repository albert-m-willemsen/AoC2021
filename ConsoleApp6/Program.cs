var exampleLines = await Runner.LoadInput("example.txt");

Console.WriteLine("Examples:");
Runner.Run(exampleLines, Day6Part1BruteForce18);
Runner.Run(exampleLines, Day6Part1BruteForce);
Runner.Run(exampleLines, Day6Part1);
Runner.Run(exampleLines, Day6Part2);

var inputLines = await Runner.LoadInput("input.txt");

Console.WriteLine("Challenges:");
Runner.Run(inputLines, Day6Part1BruteForce);
Runner.Run(inputLines, Day6Part1);
Runner.Run(inputLines, Day6Part2);


[Challenge(6, 1)]
static int Day6Part1BruteForce18(IImmutableList<string> lines) =>
    SimulateBruteForce(lines, 18).Count;

[Challenge(6, 1)]
static int Day6Part1BruteForce(IImmutableList<string> lines) =>
    SimulateBruteForce(lines, 80).Count;


[Challenge(6, 1)]
static long Day6Part1(IImmutableList<string> lines) =>
    SimulateSmart(lines, 80)
        .Sum();

[Challenge(6, 2)]
static long Day6Part2(IImmutableList<string> lines) =>
    SimulateSmart(lines, 256)
        .Sum();

static IImmutableList<int> GetState(IImmutableList<string> lines) =>
    lines[0].Split(',')
        .Select(v => Convert.ToInt32(v))
        .ToImmutableArray();

static IImmutableList<int> SimulateBruteForce(IImmutableList<string> lines, int days) =>
        Enumerable.Range(1, days)
            .Aggregate(
                GetState(lines),
                (state, _) => SimulateStepBruteForce(state)
            );

static IImmutableList<int> SimulateStepBruteForce(IImmutableList<int> state) =>
    state
        .Select(fish => fish == 0 ? 6 : fish - 1)
        .Concat(Enumerable.Repeat(8, state.Count(fish => fish == 0)))
        .ToImmutableArray();

static IImmutableList<long> GetFlattenedState(IImmutableList<string> lines) =>
    GetState(lines)
        .Aggregate(new long[9].ToImmutableArray(), (acc, fish) =>
            acc.SetItem(fish, acc[fish] + 1)
        );

static IImmutableList<long> SimulateSmart(IImmutableList<string> lines, int days) =>
    Enumerable.Range(1, days)
        .Aggregate(
            GetFlattenedState(lines),
            (state, _) => SimulateStepSmart(state)
        );

static IImmutableList<long> SimulateStepSmart(IImmutableList<long> state) =>
    Enumerable.Range(0, state.Count - 1)
        .Select(i => state[i + 1])
        .ToImmutableArray()
        .SetItem(6, state[7] + state[0])
        .Add(state[0]);
