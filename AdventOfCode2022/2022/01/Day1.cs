namespace AdventOfCode2022._2022._01;

internal class Day1 : IDay
{
    public int Year => 2022;

    public int Day => 1;

    public Task<string> Part1(string input)
    {
        var elves = input.Split(Environment.NewLine + Environment.NewLine);
        var topElf = elves.Select(e => e.Split(Environment.NewLine).Select(int.Parse).Sum()).Max();
        return Task.FromResult(topElf.ToString());
    }

    public Task<string> Part2(string input)
    {
        var elves = input.Split(Environment.NewLine + Environment.NewLine);
        var topElves = elves.Select(e => e.Split(Environment.NewLine).Select(int.Parse).Sum()).OrderByDescending(e => e)
            .ToArray();
        var sumTopElves = topElves[0] + topElves[1] + topElves[2];
        return Task.FromResult(sumTopElves.ToString());
    }

    public string? TestInput => null;
}