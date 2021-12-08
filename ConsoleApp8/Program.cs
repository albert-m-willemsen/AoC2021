var inputLines = await File.ReadAllLinesAsync("input.txt");

var lengthLookup = new Dictionary<int, int>
{
    {2, 1 },
    {3, 7 },
    {4, 4 },
    {7, 8 }
};

var splitInputs = inputLines
    .Select(line => line.Split(" | ")
        .Select(codes => codes.Split(' ')
            .Select(code => string.Join("", code.OrderBy(v => v)))
            .ToArray()
        )
        .ToArray()
    )
    .ToArray();

{
    var initialResult = new Dictionary<int, int>
    {
        {1, 0 },
        {4, 0 },
        {7, 0 },
        {8, 0 }
    };
    var result = splitInputs.Select(input => input[1])
        .SelectMany(codes => codes)
        .Aggregate(initialResult, (acc, code) =>
        {
            if (lengthLookup.TryGetValue(code.Length, out int digit))
                acc[digit] += 1;
            return acc;
        });

    Console.WriteLine(result.Values.Sum());
}

{
    var decoders = splitInputs.Select(input => input[0])
        .Select(codes => CreateDecoder(codes)).ToArray();
    var strings = splitInputs.Select(input => input[1])
        .Select((outputs, index) => outputs.Select(output => decoders[index][output]))
        .Select(numbers => string.Concat(numbers));
    var numbers = strings.Select(s => Convert.ToInt32(s));

    var result = numbers.Sum();

    Console.WriteLine(result);
}

static IDictionary<string, int> CreateDecoder(IEnumerable<string> inputs)
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
}
