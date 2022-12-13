namespace AdventOfCode2022;

internal interface IDay
{
    int Year { get; }

    int Day { get; }

    string Part1(string input);

    string Part2(string input);

    string? TestInput { get; }
}