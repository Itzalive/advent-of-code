namespace AdventOfCode2023._2023._01;

internal class Day1 : IDay
{
    public int Year => 2023;

    public int Day => 1;

    public string? Part1TestSolution => null;

    public Task<string> Part1(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var lineNumbers = lines.Select(l => l.Where(char.IsNumber));
        var calibrations = lineNumbers.Select(l => int.Parse($"{l.First()}{l.Last()}")).ToList();
        return Task.FromResult(calibrations.Sum().ToString());
    }

    public string? Part2TestSolution => null;

    public Task<string> Part2(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var numberMatch = new Dictionary<string, string>
        {
            { "one", "1" },
            { "two", "2" },
            { "three", "3" },
            { "four", "4" },
            { "five", "5" },
            { "six", "6" },
            { "seven", "7" },
            { "eight", "8" },
            { "nine", "9" }
        };

        var numbers = new List<List<string>>();
        foreach (var line in lines)
        {
            var lineNumbers = new List<string>();
            for (var i = 0; i < line.Length; i++)
            {
                foreach (var k in numberMatch)
                {
                    if (line[i..].StartsWith(k.Key) || line[i..].StartsWith(k.Value))
                    {
                        lineNumbers.Add(k.Value);
                    }
                }
            }

            numbers.Add(lineNumbers);
        }

        var calibrations = numbers.Select(l => int.Parse($"{l.First()}{l.Last()}")).ToList();
        return Task.FromResult(calibrations.Sum().ToString());
    }

    public string? TestInput => null;


}