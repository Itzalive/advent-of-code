namespace AdventOfCode2023._2023._06;

public class Day6 : IDay
{
    public int Year => 2023;
    public int Day => 6;
    
    public Task<string> Part1(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var times = lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var distances = lines[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        var score = 1;
        for (int i = 0; i < times.Length; i++) {
            var time = times[i];
            var distanceToBeat = distances[i];
            var possibleDistances = new int[time];
            for (var t = 1; t < time; t++)
            {
                possibleDistances[t] = (time - t) * t;
            }

            var numWinners = possibleDistances.Count(d => d > distanceToBeat);
            Console.WriteLine(numWinners);
            score *= numWinners;
        }

        return Task.FromResult(score.ToString());
    }

    public Task<string> Part2(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var times = lines[0].Split(':')[1].Replace(" ", "").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        var distances = lines[1].Split(':')[1].Replace(" ", "").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

        var score = 1;
        for (long i = 0; i < times.Length; i++) {
            var time = times[i];
            var distanceToBeat = distances[i];
            var numWinners = 0;
            for (long t = 1; t < time; t++)
            {
                if (time * t - t * t > distanceToBeat)
                {
                    numWinners++;
                }
            }

            Console.WriteLine(numWinners);
            score *= numWinners;
        }

        return Task.FromResult(score.ToString());
    }

    public string TestInput => @"Time:      7  15   30
Distance:  9  40  200";
}