namespace AdventOfCode2023;

internal interface IDay
{
    int Year { get; }

    int Day { get; }

    string? Part1TestSolution { get; }

    Task<string> Part1(string input);

    string? Part2TestSolution { get; }

    Task<string> Part2(string input);

    string? TestInput { get; }
}