using System.Text.RegularExpressions;

namespace AdventOfCode2023._2024._13;

public class Day13 : IDay
{
    public int Year => 2024;

    public int Day => 13;

    public string? Part1TestSolution => "480";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var regex = new Regex(
            @"Button A\: X\+([0-9]+), Y\+([0-9]+)\r\nButton B\: X\+([0-9]+), Y\+([0-9]+)\r\nPrize\: X\=([0-9]+), Y\=([0-9]+)",
            RegexOptions.Compiled);
        var score = 0L;
        foreach (var clawMachineInput in inputs)
        {
            var match = regex.Match(clawMachineInput);
            var aX = long.Parse(match.Groups[1].Value);
            var aY = long.Parse(match.Groups[2].Value);
            var bX = long.Parse(match.Groups[3].Value);
            var bY = long.Parse(match.Groups[4].Value);
            var prizeX = long.Parse(match.Groups[5].Value);
            var prizeY = long.Parse(match.Groups[6].Value);

            var options = new List<long>();

            var aYincrement = (double)prizeY / bY;
            var ratioY = (double)aY / bY;
            var a = (prizeX - bX * aYincrement) / (aX - bX * ratioY);
            var b = aYincrement - ratioY * a;
            if (Math.Abs(Math.Round(a) - a) < 0.0001 && Math.Abs(Math.Round(b) - b) < 0.0001)
            {
                Console.WriteLine($"A: {a} B: {b} Cost:{a * 3 + (long)Math.Round(b)}");
                options.Add((long)(a * 3 + (long)Math.Round(b)));
            }

            var newScore = options.Any() ? options.Min() : 0;
            score += newScore;
            Console.WriteLine($"Score: {newScore}");
            Console.WriteLine();
        }

        return Task.FromResult(score.ToString());
    }

    public string? Part2TestSolution => null;


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var regex = new Regex(
            @"Button A\: X\+([0-9]+), Y\+([0-9]+)\r\nButton B\: X\+([0-9]+), Y\+([0-9]+)\r\nPrize\: X\=([0-9]+), Y\=([0-9]+)",
            RegexOptions.Compiled);
        var score = 0L;
        foreach (var clawMachineInput in inputs)
        {
            var match = regex.Match(clawMachineInput);
            var aX = long.Parse(match.Groups[1].Value);
            var aY = long.Parse(match.Groups[2].Value);
            var bX = long.Parse(match.Groups[3].Value);
            var bY = long.Parse(match.Groups[4].Value);
            var prizeX = long.Parse(match.Groups[5].Value) + 10000000000000;
            var prizeY = long.Parse(match.Groups[6].Value) + 10000000000000;

            var options = new List<long>();

            var aYincrement = (decimal)prizeY / bY;
            var ratioY = (decimal)aY / bY;
            var a = (prizeX - bX * aYincrement) / (aX - bX * ratioY);
            var b = aYincrement - ratioY * a;
            if (Math.Abs(Math.Round(a) - a) < 0.0001m && Math.Abs(Math.Round(b) - b) < 0.0001m)
            {
                Console.WriteLine($"A: {a} B: {b} Cost:{a * 3 + (long)Math.Round(b)}");
                options.Add((long)((long)Math.Round(a) * 3 + (long)Math.Round(b)));
            }

            var newScore = options.Any() ? options.Min() : 0;
            score += newScore;
            Console.WriteLine($"Score: {newScore}");
            Console.WriteLine();
        }

        return Task.FromResult(score.ToString());
    }

    private static ulong GCD(ulong a, ulong b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
                a %= b;
            else
                b %= a;
        }

        return a | b;
    }

    public string? TestInput => @"Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279";
}