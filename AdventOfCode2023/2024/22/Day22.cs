using System.Collections.Concurrent;

namespace AdventOfCode2023._2024._22;

public class Day22 : IDay
{
    public int Year => 2024;

    public int Day => 22;

    public string? Part1TestSolution => "37327623";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(long.Parse).ToArray();
        var score = 0L;
        foreach (var seed in inputs)
        {
            var secretNumber = seed;
            for (var i = 0; i < 2000; i++)
            {
                secretNumber = ((secretNumber << 6) ^ secretNumber) % 16777216;
                secretNumber = ((secretNumber >> 5) ^ secretNumber) % 16777216;
                secretNumber = ((secretNumber << 11) ^ secretNumber) % 16777216;
            }
            Console.WriteLine(secretNumber);
            score += secretNumber;
        }

        return Task.FromResult(score.ToString());
    }

    public string? Part2TestSolution => "23";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(long.Parse).ToArray();
        var possibleScores = new ConcurrentDictionary<(long, long, long, long), long>();
        foreach (var seed in inputs)
        {
            var seenInSeed = new HashSet<(long, long, long, long)>();
            var secretNumber = seed;
            var v4 = 0L;
            var v3 = 0L;
            var v2 = 0L;
            var v1 = 0L;

            var lastDigit = secretNumber % 10;
            for (var i = 0; i < 2000; i++)
            {
                secretNumber = ((secretNumber << 6) ^ secretNumber) % 16777216;
                secretNumber = ((secretNumber >> 5) ^ secretNumber) % 16777216;
                secretNumber = ((secretNumber << 11) ^ secretNumber) % 16777216;
                v4 = v3;
                v3 = v2;
                v2 = v1;
                var newLastDigit = secretNumber % 10;
                v1 = newLastDigit - lastDigit;
                lastDigit = newLastDigit;
                if (i >= 3 && seenInSeed.Add((v4, v3, v2, v1)))
                {
                    possibleScores.AddOrUpdate((v4, v3, v2, v1), _ => newLastDigit, (_, v) => v + newLastDigit);
                }
            }
        }

        var orderedScore = possibleScores.OrderByDescending(kvp => kvp.Value).ToList();
        var maxBy = possibleScores.MaxBy(kvp => kvp.Value);
        Console.WriteLine($"{maxBy.Key.Item1},{maxBy.Key.Item2},{maxBy.Key.Item3},{maxBy.Key.Item4}");
        return Task.FromResult(maxBy.Value.ToString());
    }

    public string? TestInput => "1\r\n2\r\n3\r\n2024";
}