var inputLines = await File.ReadAllLinesAsync("input.txt");

var numbers = inputLines.Select(line => Convert.ToInt32(line)).ToList();

{
    var result = numbers.Skip(1).Where((number, index) => number > numbers[index]).Count();
    Console.WriteLine(result);
}

{
    var windows = numbers.Skip(2).Select((_, index) => numbers[index] + numbers[index + 1] + numbers[index + 2]).ToArray();
    var result = windows.Skip(1).Where((number, index) => number > windows[index]).Count();
    Console.WriteLine(result);
}