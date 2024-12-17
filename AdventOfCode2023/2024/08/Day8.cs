using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2023._2024._08;

public class Day8 : IDay
{
    public int Year => 2024;

    public int Day => 8;

    public string? Part1TestSolution => "14";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var antinodes = Enumerable.Range(0, inputs.Length)
            .Select(r => Enumerable.Range(0, inputs[r].Length).Select(c => '.').ToArray()).ToArray();
        var frequencies = new List<(char Frequency, int X, int Y)>();
        for (var y = 0; y < inputs.Length; y++)
        {
            for (var x = 0; x < inputs[y].Length; x++)
            {
                if (inputs[y][x] != '.')
                {
                    frequencies.Add((inputs[y][x], x, y));
                }
            }
        }

        foreach (var frequency in frequencies.GroupBy(v => v.Frequency).ToDictionary(g => g.Key, g => g.ToList()))
        {
            var points = frequency.Value;
            for (var i = 0; i < points.Count - 1; i++)
            {
                for (var j = i + 1; j < points.Count; j++)
                {
                    var testX = points[j].X;
                    var testY = points[j].Y;
                    do
                    {
                        testX = testX + (points[j].X - points[i].X);
                        testY = testY + (points[j].Y - points[i].Y);
                        if (testY >= 0 && testY < antinodes.Length && testX >= 0 && testX < antinodes[testY].Length)
                        {
                            antinodes[testY][testX] = '#';
                        }
                    } while (testY >= 0 && testY < antinodes.Length && testX >= 0 && testX < antinodes[testY].Length);


                    testX = points[i].X;
                    testY = points[i].Y;
                    do
                    {
                        testX = testX - (points[j].X - points[i].X);
                        testY = testY - (points[j].Y - points[i].Y);
                        if (testY >= 0 && testY < antinodes.Length && testX >= 0 && testX < antinodes[testY].Length)
                        {
                            antinodes[testY][testX] = '#';
                        }
                    } while (testY >= 0 && testY < antinodes.Length && testX >= 0 && testX < antinodes[testY].Length);
                }
            }
        }

        foreach (var line in antinodes)
        {
            Console.WriteLine(string.Join("", line));
        }

        return Task.FromResult(antinodes.Sum(l => l.Count(c => c == '#')).ToString());
    }

    public string? Part2TestSolution => "34";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var antinodes = Enumerable.Range(0, inputs.Length)
            .Select(r => Enumerable.Range(0, inputs[r].Length).Select(c => '.').ToArray()).ToArray();
        var frequencies = new List<(char Frequency, int X, int Y)>();
        for (var y = 0; y < inputs.Length; y++)
        {
            for (var x = 0; x < inputs[y].Length; x++)
            {
                if (inputs[y][x] != '.')
                {
                    frequencies.Add((inputs[y][x], x, y));
                }
            }
        }

        foreach (var frequency in frequencies.GroupBy(v => v.Frequency).ToDictionary(g => g.Key, g => g.ToList()))
        {
            var points = frequency.Value;
            for (var i = 0; i < points.Count - 1; i++)
            {
                for (var j = i + 1; j < points.Count; j++)
                {
                    var testX = points[i].X;
                    var testY = points[i].Y;
                    do
                    {
                        testX = testX + (points[j].X - points[i].X);
                        testY = testY + (points[j].Y - points[i].Y);
                        if (testY >= 0 && testY < antinodes.Length && testX >= 0 && testX < antinodes[testY].Length)
                        {
                            antinodes[testY][testX] = '#';
                        }
                    } while (testY >= 0 && testY < antinodes.Length && testX >= 0 && testX < antinodes[testY].Length);


                    testX = points[j].X;
                    testY = points[j].Y;
                    do
                    {
                        testX = testX - (points[j].X - points[i].X);
                        testY = testY - (points[j].Y - points[i].Y);
                        if (testY >= 0 && testY < antinodes.Length && testX >= 0 && testX < antinodes[testY].Length)
                        {
                            antinodes[testY][testX] = '#';
                        }
                    } while (testY >= 0 && testY < antinodes.Length && testX >= 0 && testX < antinodes[testY].Length);
                }
            }
        }

        foreach (var line in antinodes)
        {
            Console.WriteLine(string.Join("", line));
        }

        return Task.FromResult(antinodes.Sum(l => l.Count(c => c == '#')).ToString());
    }

    public string? TestInput => @"............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............";
}