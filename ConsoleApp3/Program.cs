var inputLines = await Runner.LoadInput("input.txt");

Runner.Run(inputLines, Day3Part1);
Runner.Run(inputLines, Day3Part2);

[Challenge(3, 1)]
static int Day3Part1(IImmutableList<string> lines)
{
    var rate = lines
        .Aggregate(new int[lines[0].Length].ToImmutableArray(), (acc, line) =>
            acc.Zip(
                line.Select(c => c - '0').ToArray(),
                (first, second) => first + second
            )
            .ToImmutableArray()
        )
        .Select(n => n > lines.Count / 2);
    var gamma = Convert.ToInt32(string.Concat(rate.Select(r => r ? '1' : '0')), 2);
    var epsilon = Convert.ToInt32(string.Concat(rate.Select(r => r ? '0' : '1')), 2);

    return gamma * epsilon;
}

[Challenge(3, 2)]
static int Day3Part2(IImmutableList<string> lines)
{
    var gammaLines = lines.ToImmutableArray();
    for (var i = 0; i < gammaLines[0].Length && gammaLines.Length > 1; i++)
    {
        var mostCommon = gammaLines.Aggregate(0, (acc, line) =>
            line[i] == '1' ? acc + 1 : acc
        ) >= gammaLines.Length / 2 ? '1' : '0';

        gammaLines = gammaLines
            .Where(line => line[i] == mostCommon)
            .ToImmutableArray();
    }
    var gamma = Convert.ToInt32(gammaLines[0], 2);

    var epsilonLines = lines.ToImmutableArray();
    for (var i = 0; i < epsilonLines[0].Length && epsilonLines.Length > 1; i++)
    {
        var mostCommon = epsilonLines.Aggregate(0, (acc, line) =>
            line[i] == '0' ? acc + 1 : acc
        ) <= epsilonLines.Length / 2 ? '0' : '1';

        epsilonLines = epsilonLines
            .Where(line => line[i] == mostCommon)
            .ToImmutableArray();
    }
    var epsilon = Convert.ToInt32(epsilonLines[0], 2);

    return gamma * epsilon;
}
