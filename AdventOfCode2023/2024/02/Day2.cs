namespace AdventOfCode2023._2024._02;

internal class Day2 : IDay
{
    public int Year => 2024;

    public int Day => 2;

    public string? Part1TestSolution => "2";

    public Task<string> Part1(string input)
    {
        var lines = input.Split(Environment.NewLine).Select(l => l.Split(' ').Select(int.Parse).ToArray()).ToArray();
        var safeCount = 0;
        foreach (var line in lines)
        {
            if (!IsSafe(line)) continue;
            Console.WriteLine(string.Join(" ", line));
            safeCount++;
        }
        return Task.FromResult(safeCount.ToString());
    }

    private static bool IsSafe(int[] line)
    {
        if (line[0] == line[1]) return false;
        var isDecreasing = line[0] > line[1];
        var isFailed = false;
        for (var i = 1; i < line.Length; i++)
        {
            var diff = line[i] - line[i - 1];
            if (Math.Abs(diff) is < 1 or > 3)
            {
                isFailed = true;
                break;
            }

            if (isDecreasing ^ (diff < 0))
            {
                isFailed = true;
                break;
            }
        }

        return !isFailed;
    }

    public string? Part2TestSolution => "4";

    public Task<string> Part2(string input)
    {
        var lines = input.Split(Environment.NewLine).Select(l => l.Split(' ').Select(int.Parse).ToArray()).ToArray();
        var safeCount = 0;
        foreach (var line in lines)
        {
            if (IsSafe(line))
            {
                safeCount++;
                continue;
            }

            for (var i = 0; i < line.Length; i++)
            {
                var newLine = ((int[])line.Clone()).ToList();
                newLine.RemoveAt(i);
                if (IsSafe(newLine.ToArray()))
                {
                    safeCount++;
                    break;
                }
            }
        }
        return Task.FromResult(safeCount.ToString());
    }

    public string? TestInput => "7 6 4 2 1\r\n1 2 7 8 9\r\n9 7 6 2 1\r\n1 3 2 4 5\r\n8 6 4 4 1\r\n1 3 6 7 9";


}