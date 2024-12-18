using System.Text.RegularExpressions;

namespace AdventOfCode2023._2024._14;

public class Day14 : IDay
{
    public int Year => 2024;

    public int Day => 14;

    public string? Part1TestSolution => "12";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var bathroomWidth = inputs.Length == 12 ? 11 : 101;
        var bathroomHeight = inputs.Length == 12 ? 7 : 103;
        var robots = new List<(int X, int Y, int VelocityX, int VelocityY)>();
        var regex = new Regex(@"p\=([0-9]+),([0-9]+) v\=(-?[0-9]+),(-?[0-9]+)", RegexOptions.Compiled);
        foreach (var inputRobot in inputs)
        {
            var match = regex.Match(inputRobot);
            var startX = int.Parse(match.Groups[1].Value);
            var startY = int.Parse(match.Groups[2].Value);
            var velocityX = int.Parse(match.Groups[3].Value);
            var velocityY = int.Parse(match.Groups[4].Value);
            var endX = (startX + velocityX * 0 + bathroomWidth * 0) % bathroomWidth;
            var endY = (startY + velocityY * 0 + bathroomHeight * 0) % bathroomHeight;
            robots.Add((endX, endY, velocityX < 0 ? velocityX + bathroomWidth : velocityX,
                velocityY < 0 ? velocityY + bathroomHeight : velocityY));
        }

        for (var i = 1;; i++)
        {
            robots = robots.Select(r => ((r.X + r.VelocityX) % bathroomWidth, (r.Y + r.VelocityY) / bathroomHeight,
                r.VelocityX, r.VelocityY)).ToList();

            Console.WriteLine(i);
            for (var y = 0; y < bathroomHeight; y++)
            {
                for (var x = 0; x < bathroomWidth; x++)
                {
                    Console.Write(robots.Count(r => r.X == x && r.Y == y));
                }

                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        var quad1 = robots.Count(r => r.X <= (bathroomWidth - 3) / 2 && r.Y <= (bathroomHeight - 3) / 2);
        var quad2 = robots.Count(r => r.X <= (bathroomWidth - 3) / 2 && r.Y >= (bathroomHeight + 1) / 2);
        var quad3 = robots.Count(r => r.X >= (bathroomWidth + 1) / 2 && r.Y <= (bathroomHeight - 3) / 2);
        var quad4 = robots.Count(r => r.X >= (bathroomWidth + 1) / 2 && r.Y >= (bathroomHeight + 1) / 2);
        Console.WriteLine($"Q1:{quad1} Q2:{quad2} Q3:{quad3} Q4:{quad4}");

        return Task.FromResult((quad1 * quad2 * quad3 * quad4).ToString());
    }

    public string? Part2TestSolution => null;


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var bathroomWidth = inputs.Length == 12 ? 11 : 101;
        var bathroomHeight = inputs.Length == 12 ? 7 : 103;
        var robots = new List<(int X, int Y, int VelocityX, int VelocityY)>();
        var regex = new Regex(@"p\=([0-9]+),([0-9]+) v\=(-?[0-9]+),(-?[0-9]+)", RegexOptions.Compiled);

        var initialSeconds = 0;
        var secondIncrement = 9;

        foreach (var inputRobot in inputs)
        {
            var match = regex.Match(inputRobot);
            var startX = int.Parse(match.Groups[1].Value);
            var startY = int.Parse(match.Groups[2].Value);
            var velocityX = int.Parse(match.Groups[3].Value);
            var velocityY = int.Parse(match.Groups[4].Value);
            var endX = (startX + velocityX * initialSeconds + bathroomWidth * initialSeconds) % bathroomWidth;
            var endY = (startY + velocityY * initialSeconds + bathroomHeight * initialSeconds) % bathroomHeight;
            robots.Add((endX, endY, velocityX < 0 ? velocityX + bathroomWidth : velocityX,
                velocityY < 0 ? velocityY + bathroomHeight : velocityY));
        }

        var carryOn = true;
        var seconds = 0;
        var maxClosest = 0;
        for (var i = 1; carryOn; i++)
        {
            var progress = 1;//20 + i * 9;
            robots = robots.Select(r => ((r.X + progress * r.VelocityX) % bathroomWidth, (r.Y + progress * r.VelocityY) % bathroomHeight,
                r.VelocityX, r.VelocityY)).ToList();

            var closest = robots.Sum(r => robots.Count(r2 => Math.Abs(r.Y - r2.Y) < 2 && Math.Abs(r.X - r2.X) < 2));
            if (closest > maxClosest)
            {
                maxClosest = closest;
                Console.WriteLine(i);
                for (var y = 0; y < bathroomHeight; y++)
                {
                    for (var x = 0; x < bathroomWidth; x++)
                    {
                        var count = robots.Count(r => r.X == x && r.Y == y);
                        if (count > 0)
                            Console.BackgroundColor = ConsoleColor.Green;
                        else
                            Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(count > 0 ? "#" : ".");
                    }

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        var quad1 = robots.Count(r => r.X <= (bathroomWidth - 3) / 2 && r.Y <= (bathroomHeight - 3) / 2);
        var quad2 = robots.Count(r => r.X <= (bathroomWidth - 3) / 2 && r.Y >= (bathroomHeight + 1) / 2);
        var quad3 = robots.Count(r => r.X >= (bathroomWidth + 1) / 2 && r.Y <= (bathroomHeight - 3) / 2);
        var quad4 = robots.Count(r => r.X >= (bathroomWidth + 1) / 2 && r.Y >= (bathroomHeight + 1) / 2);
        Console.WriteLine($"Q1:{quad1} Q2:{quad2} Q3:{quad3} Q4:{quad4}");

        return Task.FromResult((quad1 * quad2 * quad3 * quad4).ToString());
    }

    public string? TestInput => @"p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3";
}