var inputLines = await File.ReadAllLinesAsync("input.txt");

/// <summary>
/// Day 3, part 1.
/// </summary>
Performance.Measure(() =>
{
    var rate = inputLines
        .Aggregate(new int[inputLines[0].Length], (acc, line) =>
            acc.Zip(
                line.Select(c => c - '0').ToArray(),
                (first, second) => first + second
            )
            .ToArray()
        )
        .Select(n => n > inputLines.Length / 2);
    var gamma = Convert.ToInt32(string.Concat(rate.Select(r => r ? '1' : '0')), 2);
    var epsilon = Convert.ToInt32(string.Concat(rate.Select(r => r ? '0' : '1')), 2);

    return gamma * epsilon;
});

/// <summary>
/// Day 3, part 2.
/// </summary>
Performance.Measure(() =>
{
    var gammaLines = inputLines.ToArray();
    for (var i = 0; i < gammaLines[0].Length && gammaLines.Length > 1; i++)
    {
        var mostCommon = gammaLines
            .Aggregate(0, (acc, line) => line[i] == '1' ? acc + 1 : acc)
            >= gammaLines.Length / 2 ? '1' : '0';

        gammaLines = gammaLines
            .Where(line => line[i] == mostCommon)
            .ToArray();
    }
    var gamma = Convert.ToInt32(gammaLines[0], 2);

    var epsilonLines = inputLines.ToArray();
    for (var i = 0; i < inputLines[0].Length && epsilonLines.Length > 1; i++)
    {
        var mostCommon = epsilonLines
            .Aggregate(0, (acc, line) => line[i] == '0' ? acc + 1 : acc)
            <= epsilonLines.Length / 2 ? '0' : '1';

        epsilonLines = epsilonLines
            .Where(line => line[i] == mostCommon)
            .ToArray();
    }
    var epsilon = Convert.ToInt32(epsilonLines[0], 2);

    return gamma * epsilon;
});
