namespace AdventOfCode2023;

internal interface IDay
{
    int Year { get; }

    int Day { get; }

    Task<string> Part1(string input);

    Task<string> Part2(string input);

    string? TestInput { get; }
}