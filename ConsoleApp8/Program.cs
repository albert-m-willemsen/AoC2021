var inputLines = await Runner.LoadInput("input.txt");

Runner.Run(inputLines, Day8Part1);
Runner.Run(inputLines, Day8Part2);

[Challenge(8, 1)]
static int Day8Part1(IImmutableList<string> lines) =>
    getCodesLines(lines)
        .SelectMany(line => line.outputs)
        .Aggregate(
            new Dictionary<int, int>
            {
                {1, 0 },
                {4, 0 },
                {7, 0 },
                {8, 0 }
            },
            (acc, code) =>
            {
                if (getLengthLookup().TryGetValue(code.Length, out int digit))
                    acc[digit] += 1;
                return acc;
            }
        )
        .Values
        .Sum();

[Challenge(8, 2)]
static int Day8Part2(IImmutableList<string> lines)
{
    var codeLines = getCodesLines(lines);
    var decoders = codeLines
        .Select(line => line.inputs)
        .Select(input => createDecoder(input))
        .ToArray();
    var strings = codeLines
        .Select(line => line.outputs)
        .Select((output, index) => output
            .Select(output => decoders[index][output])
        )
        .Select(numbers => string.Concat(numbers));
    var numbers = strings.Select(s => Convert.ToInt32(s));

    return numbers.Sum();
}

static IImmutableDictionary<int, int> getLengthLookup() =>
    new Dictionary<int, int>
    {
        {2, 1 },
        {3, 7 },
        {4, 4 },
        {7, 8 }
    }
    .ToImmutableDictionary();

static IImmutableList<(ImmutableArray<string> inputs, ImmutableArray<string> outputs)> getCodesLines(IImmutableList<string> lines) =>
    lines.Select(line => line.Split(" | ")
        .Select(codes => codes.Split(' ')
            .Select(code => string.Concat(code.OrderBy(v => v)))
            .ToImmutableArray()
        )
        .ToImmutableArray()
    )
    .Select(line => (line[0], line[1]))
    .ToImmutableArray();

static IDictionary<string, int> createDecoder(IEnumerable<string> inputs)
{
    var codes = inputs.OrderBy(code => code.Length).ToArray();
    var encoder = new Dictionary<int, string>
        {
            { 1, codes[0] },
            { 4, codes[2] },
            { 7, codes[1] },
            { 8, codes[9] }
        };

    var code690 = codes.Where(code => code.Length == 6);
    encoder.Add(6, code690.Single(code => !encoder[7].All(wire => code.Contains(wire))));
    encoder.Add(9, code690.Single(code => encoder[4].All(wire => code.Contains(wire))));
    encoder.Add(0, code690.Single(code => code != encoder[6] && code != encoder[9]));

    var code235 = codes.Where(code => code.Length == 5);
    encoder.Add(3, code235.Single(code => encoder[1].All(wire => code.Contains(wire))));
    encoder.Add(5, code235.Single(code => code.All(wire => encoder[6].Contains(wire))));
    encoder.Add(2, code235.Single(code => code != encoder[3] && code != encoder[5]));

    return encoder.OrderBy(k => k.Key).ToDictionary(k => k.Value, v => v.Key);
};
