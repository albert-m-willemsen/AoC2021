using Shared;

var inputLines = await Runner.LoadInput("input.txt");

Runner.Run(inputLines, Day10Part1);
Runner.Run(inputLines, Day10Part2);

[Challenge(10, 1)]
static int Day10Part1(IImmutableList<string> lines) =>
    lines.Aggregate(ImmutableArray<char>.Empty, (acc, line) =>
        tryValidateLine(line, out char error)
        ? acc
        : acc.Append(error).ToImmutableArray()
    )
    .Select((error) => score1(error))
    .Sum();

[Challenge(10, 2)]
static long Day10Part2(IImmutableList<string> lines) =>
    lines.Where(line => tryValidateLine(line, out char _))
        .Select(line => completeLine(line))
        .Select(line => line
            .Aggregate(0L, (acc, c) => acc * 5 + score2(c))
        )
        .OrderBy(v => v)
        .Middle();

static int score1(char c) => c switch
{
    ')' => 3,
    ']' => 57,
    '}' => 1197,
    '>' => 25137,
    _ => throw new NotImplementedException()
};

static int score2(char c) => c switch
{
    ')' => 1,
    ']' => 2,
    '}' => 3,
    '>' => 4,
    _ => throw new NotImplementedException()
};

static bool isCorrectTag(char b, char e) => e switch
{
    ')' => b == '(',
    ']' => b == '[',
    '}' => b == '{',
    '>' => b == '<',
    _ => false
};

static bool tryValidateLine(string line, out char error)
{
    error = '\0';
    var stack = new Stack<char>();

    for (var i = 0; i < line.Length; i++)
    {
        var c = line[i];
        if (c == '(' || c == '[' || c == '{' || c == '<')
            stack.Push(c);
        else if (c == ')' || c == ']' || c == '}' || c == '>')
        {
            if (!stack.TryPop(out char b) || !isCorrectTag(b, c))
            {
                error = c;
                return false;
            }
        }
        else
            throw new ArgumentException($"{nameof(c)} = '{c}':{(byte)c}");
    }

    return true;
};

static char getCorrectTag(char tag) => tag switch
{
    '(' => ')',
    '[' => ']',
    '{' => '}',
    '<' => '>',
    _ => throw new NotImplementedException()
};

static string completeLine(string line)
{
    var stack = new Stack<char>();
    for (var i = 0; i < line.Length; i++)
    {
        var c = line[i];
        if (c == '(' || c == '[' || c == '{' || c == '<')
            stack.Push(c);
        else if (c == ')' || c == ']' || c == '}' || c == '>')
            stack.Pop();
    }

    return string.Concat(stack.Select(c => getCorrectTag(c)));
};
