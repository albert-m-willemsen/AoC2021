var inputLines = await File.ReadAllLinesAsync("input.txt");

/// <summary>
/// Day 1, part 1.
/// </summary>
Performance.Measure(() =>
{
    var numbers = getNumbers(inputLines);

    return numbers.Skip(1)
        .Where((number, index) => number > numbers[index])
        .Count();
});

/// <summary>
/// Day 1, part 1.
/// </summary>
Performance.Measure(() =>
{
    var numbers = getNumbers(inputLines);

    var windows = numbers.Skip(2)
        .Select((_, index) => numbers[index] + numbers[index + 1] + numbers[index + 2])
        .ToArray();
    return windows.Skip(1)
        .Where((number, index) => number > windows[index])
        .Count();
});

static IList<int> getNumbers(string[] lines)
    => lines.Select(line => Convert.ToInt32(line)).ToList();
