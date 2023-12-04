namespace AdventOfCode2023._2022._04;

internal class Day4 : IDay
{
    public int Year => 2022;
    public int Day => 4;

    public async Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var assignments = inputs
            .Select(i => i.Split(",").Select(j => j.Split("-").Select(int.Parse).ToArray()).ToArray()).ToArray();
        return assignments
            .Count(a => a[0][0] <= a[1][0] && a[0][1] >= a[1][1] || a[0][0] >= a[1][0] && a[0][1] <= a[1][1])
            .ToString();
    }

    public async Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var assignments = inputs
            .Select(i => i.Split(",").Select(j => j.Split("-").Select(int.Parse).ToArray()).ToArray()).ToArray();
        return assignments.Count(a =>
            a[0][0] <= a[1][0] && a[0][1] >= a[1][1] || a[0][0] >= a[1][0] && a[0][1] <= a[1][1] ||
            a[1][1] >= a[0][0] && a[0][1] >= a[1][0] || a[0][1] >= a[1][0] && a[1][1] >= a[0][0]).ToString();
    }

    public string TestInput => @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8";
}