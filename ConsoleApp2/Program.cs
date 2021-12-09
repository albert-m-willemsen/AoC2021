var inputLines = await File.ReadAllLinesAsync("input.txt");

var inputs = inputLines
    .Select(line =>
    {
        var matches = line.Split(' ');

        return (matches[0], Convert.ToInt32(matches[1]));
    })
    .ToArray();

{
    var result = inputs
        .Aggregate((0, 0), (acc, input) =>
        {
            var (x, y) = acc;
            var (direction, magnitude) = input;
            return direction switch
            {
                "up" => (x, y - magnitude),
                "down" => (x, y + magnitude),
                "forward" => (x + magnitude, y),
                _ => throw new ArgumentException(nameof(direction)),
            };
        });
    var (x, y) = result;

    Console.WriteLine(x * y);
};

{
    var result = inputs
        .Aggregate((0, 0, 0), (acc, input) =>
        {
            var (x, y, aim) = acc;
            var (direction, magnitude) = input;
            return direction switch
            {
                "up" => (x, y, aim - magnitude),
                "down" => (x, y, aim + magnitude),
                "forward" => (x + magnitude, y + aim * magnitude, aim),
                _ => throw new ArgumentException(nameof(direction)),
            };
        });
    var (x, y, aim) = result;

    Console.WriteLine(x * y);
};
