var inputLines = await File.ReadAllLinesAsync("input.txt");

var isCorrectTag = (char b, char e) => b switch
{
    '(' => e == ')',
    '[' => e == ']',
    '{' => e == '}',
    '<' => e == '>',
    _ => false
};
var tryValidateLine = (string line, out char error) =>
{
    error = '\0';
    var stack = new Stack<char>();

    var length = line.Length;
    for (var i = 0; i < length; i++)
    {
        error = line[i];
        if (error == '(' || error == '[' || error == '{' || error == '<')
            stack.Push(error);
        else if (error == ')' || error == ']' || error == '}' || error == '>')
        {
            if (!stack.TryPop(out char b) || !isCorrectTag(b, error))
                return false;
        }
        else
            throw new ArgumentException($"{nameof(error)} = {error}:{(byte)error}");
    }

    return true;
};

{
    var score = (char c) => c switch
    {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => throw new ArgumentException($"{nameof(c)} = {c}:{(byte)c}")
    };
    var result = inputLines
        .Aggregate<string, char[]>(Array.Empty<char>(), (char[] acc, string line) => !tryValidateLine(line, out char error) ? acc : acc.Concat(new char[] { error }).ToArray())
        .Select<char, int>((char error) => score(error))
        .Sum();

    Console.WriteLine(result);
}

{
    var score = (char c) => c switch
    {
        ')' => 1,
        ']' => 2,
        '}' => 3,
        '>' => 4,
        _ => throw new ArgumentException($"{nameof(c)} = {c}:{(byte)c}")
    };
    var getCorrectTag = (char tag) => tag switch
        {
            '(' => ')',
            '[' => ']',
            '{' => '}',
            '<' => '>',
            _ => throw new ArgumentException($"{nameof(tag)} = {tag}:{(byte)tag}")
        };
    var completeLine = (string line) =>
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

    var result = inputLines
        .Where(line => tryValidateLine(line, out char _))
        .Select(line => completeLine(line))
        .Select(line => line
            .Aggregate(0L, (acc, c) =>
            {
                acc = acc * 5 + score(c);
                return acc;
            })
        )
        .OrderBy(v => v)
        .ToArray();


    Console.WriteLine(result[(result.Length) / 2]);
}