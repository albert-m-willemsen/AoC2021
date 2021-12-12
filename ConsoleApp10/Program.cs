var inputLines = await File.ReadAllLinesAsync("input.txt");

/// <summary>
/// Day 10, part 1.
/// </summary>
Performance.Measure(() =>
{
    return inputLines
        .Aggregate(Array.Empty<char>(), (acc, line) =>
            tryValidateLine(line, out char error)
                ? acc
                : acc.Concat(new char[] { error }).ToArray())
        .Select((error) => score1(error))
        .Sum();
});

/// <summary>
/// Day 10, part 2.
/// </summary>
Performance.Measure(() =>
{
    var lineCompletions = inputLines
        .Where(line => tryValidateLine(line, out char _))
        .Select(line => completeLine(line))
        .Select(line => line
            .Aggregate(0L, (acc, c) => acc * 5 + score2(c))
        )
        .OrderBy(v => v)
        .ToArray();

    return lineCompletions[(lineCompletions.Length) / 2];
});

static int score1(char c) => c switch
{
    ')' => 3,
    ']' => 57,
    '}' => 1197,
    '>' => 25137,
    _ => throw new ArgumentException($"{nameof(c)} = '{c}':{(byte)c}")
};

static int score2(char c) => c switch
{
    ')' => 1,
    ']' => 2,
    '}' => 3,
    '>' => 4,
    _ => throw new ArgumentException($"{nameof(c)} = '{c}':{(byte)c}")
};

static bool isCorrectTag(char b, char e) => b switch
{
    '(' => e == ')',
    '[' => e == ']',
    '{' => e == '}',
    '<' => e == '>',
    _ => false
};

static bool tryValidateLine(string line, out char error)
{
    error = '\0';
    var stack = new Stack<char>();

    var length = line.Length;
    for (var i = 0; i < length; i++)
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
    _ => throw new ArgumentException($"{nameof(tag)} = '{tag}':{(byte)tag}")
};

static string completeLine(string line)
{
    var stack = new Stack<char>();
    var lineLength = line.Length;
    for (var i = 0; i < lineLength; i++)
    {
        var c = line[i];
        if (c == '(' || c == '[' || c == '{' || c == '<')
            stack.Push(c);
        else if (c == ')' || c == ']' || c == '}' || c == '>')
            stack.Pop();
    }

    return string.Concat(stack.Select(c => getCorrectTag(c)));
};
