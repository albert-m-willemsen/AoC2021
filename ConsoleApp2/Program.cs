var inputLines = await Runner.LoadInput("input.txt");

Runner.Run(inputLines, Day2Part1);
Runner.Run(inputLines, Day2Part2);

[Challenge(2, 1)]
static int Day2Part1(IImmutableList<string> lines) =>
    GetInputs(lines)
        .Aggregate((x: 0, y: 0), (acc, input) =>
        {
            var (x, y) = acc;
            var (direction, magnitude) = input;
            return direction switch
            {
                Direction.up => (x, y - magnitude),
                Direction.down => (x, y + magnitude),
                Direction.forward => (x + magnitude, y),
                _ => throw new ArgumentException(nameof(direction)),
            };
        }, r => r.x * r.y);

[Challenge(2, 2)]
static int Day2Part2(IImmutableList<string> lines) =>
    GetInputs(lines)
        .Aggregate((x: 0, y: 0, aim: 0), (acc, input) =>
        {
            var (x, y, aim) = acc;
            var (direction, magnitude) = input;
            return direction switch
            {
                Direction.up => (x, y, aim - magnitude),
                Direction.down => (x, y, aim + magnitude),
                Direction.forward => (x + magnitude, y + aim * magnitude, aim),
                _ => throw new ArgumentException(nameof(direction)),
            };
        }, r => r.x * r.y);

static IImmutableList<(Direction direction, int magnitude)> GetInputs(IImmutableList<string> lines) =>
    lines.SelectMany(line => line.Split(' ')
        .Chunk(2)
        .Select(match => (Enum.Parse<Direction>(match[0]), Convert.ToInt32(match[1])))
    )
    .ToImmutableArray();

enum Direction { up, down, forward }