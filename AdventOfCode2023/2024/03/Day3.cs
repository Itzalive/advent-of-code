using System.Text.RegularExpressions;

namespace AdventOfCode2023._2024._03;

public class Day3 : IDay
{
    public int Year => 2024;

    public int Day => 3;

    public string? Part1TestSolution => "161";

    public Task<string> Part1(string input)
    {
        var matches = Regex.Matches(input, "mul\\(([0-9]+),([0-9]+)\\)");
        var result = matches.Select(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value)).Sum();
        return Task.FromResult(result.ToString());
    }

    public string? Part2TestSolution => "48";


    public Task<string> Part2(string input)
    {
        var matches = Regex.Matches(input, "(mul\\(([0-9]+),([0-9]+)\\)|do\\(\\)|don't\\(\\))");
        var sum = 0;
        var enabled = true;
        foreach (Match match in matches)
        {
            if (enabled && match.Value.StartsWith("mul"))
            {
                sum += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
            }

            if (match.Value.StartsWith("do()"))
            {
                enabled = true;
            }

            if (match.Value.StartsWith("don't()"))
            {
                enabled = false;
            }
        } 
        return Task.FromResult(sum.ToString());
    }

    public string? TestInput => "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";//"xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
}