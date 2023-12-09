namespace AdventOfCode2023._2023._09;

public class Day9 : IDay
{
    public int Year => 2023;
    public int Day => 9;

    public string? Part1TestSolution => "114";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var nextValue = inputs.Select(l => l.Split(' ').Select(int.Parse).ToArray()).Select(l => SolveForNext(l));
        return Task.FromResult(nextValue.Sum().ToString());
    }

    private int SolveForNext(int[] ints)
    {
        var results = new int[ints.Length - 1];
        for (var i = 0; i < ints.Length - 1; i++)
        {
            results[i] = ints[i + 1] - ints[i];
        }

        if (results.Any(r => r != 0))
        {
            return ints[^1] + SolveForNext(results);
        }

        return ints[^1];
    }

    public string? Part2TestSolution => "2";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var previousValue = inputs.Select(l => l.Split(' ').Select(int.Parse).ToArray()).Select(l => SolveBack(l));
        return Task.FromResult(previousValue.Sum().ToString());
    }
    

    private int SolveBack(int[] ints)
    {
        var results = new int[ints.Length - 1];
        for (var i = 0; i < ints.Length - 1; i++)
        {
            results[i] = ints[i + 1] - ints[i];
        }

        if (results.Any(r => r != 0))
        {
            return ints[0] - SolveBack(results);
        }

        return ints[0];
    }

    public string? TestInput => @"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45";
}