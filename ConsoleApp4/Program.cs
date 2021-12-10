﻿using System.Text.RegularExpressions;

var inputLines = await File.ReadAllLinesAsync("input.txt");

var numbers = inputLines[0]
    .Split(',')
    .Select(s => Convert.ToInt32(s))
    .ToArray();

var boards = inputLines
    .Skip(1)
    .Select((v, i) => (v, i))
    .GroupBy(t => t.i / 6)
    .Select(group => group
        .Skip(1)
        .Select(t => Regex.Matches(t.v, @"\d+")
            .Select(m => m.Value)
            .Select(v => Convert.ToInt32(v))
            .ToArray()
        )
        .ToArray()
    )
    .Select(board => new Board(board))
    .ToArray();

{
    var play = (int[] numbers, Board[] boards) =>
    {
        foreach (var number in numbers)
            foreach (var board in boards)
                if (board.Mark(number))
                    return board.Unmarked().Sum() * number;

        throw new InvalidOperationException();
    };

    var result = play(numbers, boards);
    Console.WriteLine(result);
}
{
    var play = (int[] numbers, Board[] boards) =>
    {
        var boardsWon = new int[boards.Length];
        foreach (var number in numbers)
            foreach (var board in boards)
            {
                board.Mark(number);
                if (boards.All(b => b.Won))
                    return board.Unmarked().Sum() * number;
            }

        throw new InvalidOperationException();
    };

    var result = play(numbers, boards);
    Console.WriteLine(result);
}

class Board
{
    readonly IDictionary<int, (int x, int y)> lookup;
    readonly int[][] board;
    readonly bool[][] marks;
    readonly int width;
    readonly int height;

    public Board(int[][] board)
    {
        this.board = board;

        width = board[0].Length;
        height = board.Length;

        lookup = new Dictionary<int, (int x, int y)>(width * height);
        for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                lookup.Add(board[y][x], (x, y));

        marks = new bool[height][];
        for (int i = 0; i < height; i++)
            marks[i] = new bool[width];
    }

    public bool Won { get; private set; }

    public bool Mark(int number)
    {
        if (lookup.ContainsKey(number))
        {
            var (x, y) = lookup[number];
            marks[y][x] = true;

            return Check(x, y);
        }

        return false;
    }

    public bool Check(int x, int y)
    {
        var won = marks.Select(row => row[x]).All(v => v)
            || marks[y].All(v => v);
        if (won)
            Won = true;

        return won;
    }

    public IEnumerable<int> Unmarked()
    {
        for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                if (!marks[y][x])
                    yield return board[y][x];
    }
}
