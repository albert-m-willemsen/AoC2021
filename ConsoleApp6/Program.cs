var inputLines = await File.ReadAllLinesAsync("input.txt");

/// <summary>
/// Day 6, part 1.
/// </summary>
Performance.Measure(() => Enumerable.Range(1, 80)
    .Aggregate(
        getState(inputLines),
        (state, _) => simulateBruteForce(state)
    )
    .Length
);

/// <summary>
/// Day 6, part 1.
/// </summary>
Performance.Measure(() => Enumerable.Range(1, 80)
    .Aggregate(
        getFlattenedState(inputLines),
        (state, _) => simulateSmart(state)
    )
    .Sum()
);

/// <summary>
/// Day 6, part 2.
/// </summary>
Performance.Measure(() => Enumerable.Range(1, 256)
        .Aggregate(
            getFlattenedState(inputLines),
            (state, _) => simulateSmart(state)
        )
        .Sum()
);

static byte[] getState(string[] lines) => lines[0].Split(',')
        .Select(v => Convert.ToByte(v))
        .ToArray();

static long[] getFlattenedState(string[] lines) => getState(lines)
    .Aggregate(new long[9], (acc, fish) =>
    {
        acc[fish] += 1;
        return acc;
    });

static byte[] simulateBruteForce(byte[] state)
{
    var length = state.Length;
    var toAppend = 0;
    for (var i = 0; i < length; i++)
    {
        var fish = state[i];
        if (fish == 0)
        {
            state[i] = 6;
            toAppend += 1;
        }
        else
            state[i] -= 1;
    }

    return state
        .Concat(Enumerable.Repeat((byte)8, toAppend))
        .ToArray();
};

static long[] simulateSmart(long[] state)
{
    var adultFish = state[0];
    for (var i = 0; i < 8; i++)
        state[i] = state[i + 1];

    state[6] += adultFish;
    state[8] = adultFish;

    return state;
};
