using System.Collections.Generic;

namespace AdventOfCode2023._2024._7;

public class Day7 : IDay
{
    public int Year => 2024;

    public int Day => 7;

    public string? Part1TestSolution => "3749";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        long result = 0;
        foreach (var line in inputs)
        {
            var lineSplit = line.Split(':');
            var target = long.Parse(lineSplit[0]);
            var values = lineSplit[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
            var currentValues = new long[] { values[0] };
            var currentIndex = 1;
            currentValues = AddRules(values, currentIndex, currentValues);
            if (currentValues.Contains(target))
            {
                result += target;
            }
        }

        return Task.FromResult(result.ToString());
    }

    private static long[] AddRules(long[] valueInputs, int currentIndex, long[] currentValues)
    {
        if (currentIndex >= valueInputs.Length)
        {
            return currentValues;
        }

        var results = new long[currentValues.Length * 2];
        for (var i = 0; i < currentValues.Length; i++)
        {
            results[i * 2 + 0] = currentValues[i] * valueInputs[currentIndex];
            //results[i * 4 + 1] = currentValues[i] / valueInputs[currentIndex];
            results[i * 2 + 1] = currentValues[i] + valueInputs[currentIndex];
            //results[i * 4 + 3] = currentValues[i] - valueInputs[currentIndex];
        }

        return AddRules(valueInputs, currentIndex + 1, results);
    }

    public string? Part2TestSolution => "11387";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        long result = 0;
        foreach (var line in inputs)
        {
            var lineSplit = line.Split(':');
            var target = long.Parse(lineSplit[0]);
            var values = lineSplit[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
            var currentIndex = 1;
            var results = AddRules2(values, currentIndex, values[0]);
            if (results.Contains(target))
            {
                result += target;
            }
        }

        return Task.FromResult(result.ToString());
    }

    private static List<long> AddRules2(long[] valueInputs, int currentIndex, long currentValues)
    {
        if (currentIndex >= valueInputs.Length)
        {
            return [currentValues];
        }

        var results = new List<long>();
        results.AddRange(AddRules2(valueInputs, currentIndex + 1, currentValues * valueInputs[currentIndex]));
        results.AddRange(AddRules2(valueInputs, currentIndex + 1, currentValues + valueInputs[currentIndex]));
        results.AddRange(AddRules2(valueInputs, currentIndex + 1,
            long.Parse($"{currentValues}{valueInputs[currentIndex]}")));

        return results;
    }

    public string? TestInput => @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20";
}