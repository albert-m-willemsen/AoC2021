﻿using System.Diagnostics;

var inputLines = await File.ReadAllLinesAsync("input.txt");

var state = inputLines[0].Split(',').Select(v => Convert.ToInt32(v));
var flattenedState = state.Aggregate(new long[9], (acc, fish) =>
{
    acc[fish] += 1;
    return acc;
});

var simulate = (long[] state) =>
{
    var adultFish = state[0];
    for (var i = 0; i < 8; i++)
        state[i] = state[i + 1];

    state[6] += adultFish;
    state[8] = adultFish;

    return state;
};


var sw = new Stopwatch();

{
    var bruteForce = (IList<int> state) =>
    {
        var length = state.Count;
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

        var result = state.AsEnumerable();
        for (; toAppend > 0; toAppend--)
            result = result.Append(8);

        return result.ToList();
    };

    sw.Restart();

    IList<int> initialState = state.ToList();
    var result = Enumerable.Range(1, 80).Aggregate(initialState, (state, _) => bruteForce(state));

    sw.Stop();
    Console.WriteLine($"{result.Count} : {sw.ElapsedMilliseconds}");
}

{
    sw.Restart();

    var initialState = flattenedState.ToArray();
    var result = Enumerable.Range(1, 80).Aggregate(initialState, (state, _) => simulate(state));

    sw.Stop();
    Console.WriteLine($"{result.Sum()} : {sw.ElapsedMilliseconds}");
}

{
    sw.Restart();

    var initialState = flattenedState.ToArray();
    var result = Enumerable.Range(1, 256).Aggregate(initialState, (state, _) => simulate(state));

    sw.Stop();
    Console.WriteLine($"{result.Sum()} : {sw.ElapsedMilliseconds}");
}
