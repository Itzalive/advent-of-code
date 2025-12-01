namespace AdventOfCode2023._2025._01;

internal class Day1 : IDay
{
    public int Year => 2025;

    public int Day => 1;

    public string? Part1TestSolution => "3";

    public Task<string> Part1(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var dial = 50;
        var count = 0;
        foreach (var line in lines)
        {
            var num = int.Parse(line[1..]);
            if (line.StartsWith('L'))
            {
                dial -= num;
                while (dial < 0)
                {
                    dial += 100;
                }
            }
            else
            {
                dial += num;
                while (dial >= 100)
                {
                    dial -= 100;
                }
            }

            if (dial == 0)
            {
                count++;
            }
        }

        return Task.FromResult(count.ToString());
    }

    public string? Part2TestSolution => "6";

    public Task<string> Part2(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var dial = 50;
        var count = 0;
        foreach (var line in lines)
        {
            var num = int.Parse(line[1..]);
            if (line.StartsWith('L'))
            {
                dial -= num;

                while (dial < 0)
                {
                    dial += 100;
                    count++;
                }
            }
            else
            {
                dial += num;
                while (dial >= 100)
                {
                    count++;
                    dial -= 100;
                }
            }
        }

        if (dial == 0)
        {
            count++;
        }

        return Task.FromResult(count.ToString());
    }

    public string? TestInput => "L68\r\nL30\r\nR48\r\nL5\r\nR60\r\nL55\r\nL1\r\nL99\r\nR14\r\nL82";


}