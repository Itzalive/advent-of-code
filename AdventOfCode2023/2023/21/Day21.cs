using System.Diagnostics.Metrics;

namespace AdventOfCode2023._2023._21;

public class Day21 : IDay
{
    public int Year => 2023;

    public int Day => 21;

    public string? Part1TestSolution => "16";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l =>
        {
            var line = new char[l.Length + 2];
            line[0] = '.';
            l.ToCharArray().CopyTo(line, 1);
            line[-1] = '.';
            return line;
        }).ToArray();
        var inputsBigger = new char[inputs.Length + 2][];
        inputsBigger[0] = Enumerable.Range(0, inputs[0].Length).Select(n => '.').ToArray();
        inputs.CopyTo(inputsBigger, 1);
        inputsBigger[-1] = Enumerable.Range(0, inputs[0].Length).Select(n => '.').ToArray();

        inputs = inputsBigger;

        var startPos = Array.Empty<int>();
        for (var y = 0; y < inputs.Length; y++)
        {
            for (var x = 0; x < inputs[y].Length; x++)
            {
                if (inputs[y][x] == 'S')
                {
                    startPos = new int[] { y, x };
                }
            }
        }

        var currentOuterRim = new List<int[]> { startPos };
        var numSteps = (inputs.Length > 10 ? 64 : 6);

        var counts = CountOnMap(inputs, numSteps, currentOuterRim, true);

        return Task.FromResult(counts[numSteps % 2].ToString());
    }

    public string? Part2TestSolution => "81922";

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();

        var startPos = Array.Empty<int>();
        for (var y = 0; y < inputs.Length; y++)
        {
            for (var x = 0; x < inputs[y].Length; x++)
            {
                if (inputs[y][x] == 'S')
                {
                    inputs[y][x] = '.';
                    startPos = new int[] { y, x };
                }
            }
        }

        var currentOuterRim = new List<int[]> { startPos };
        var countsComplete = CountOnMap(inputs, inputs.Length * 5, currentOuterRim, true);

        var corners = new List<int[]>
        {
            new[] { 0, 0 },
            new[] { inputs.Length - 1, 0 },
            new[] { inputs.Length - 1, inputs[0].Length - 1 },
            new[] { 0, inputs[0].Length - 1 }
        };
        var countsCorners = CountOnMap(inputs, (inputs.Length - 1) / 2 - 1, corners, true);
        //countsCorners[0] += 4;

        var countsCornersExp = new long[] { 0, 0 };
        foreach (var corner in corners)
        {
            var c = CountOnMap(inputs, inputs.Length - 1 + (inputs.Length - 1) / 2, new List<int[]> { corner }, true);
            countsCornersExp[0] += c[0];
            countsCornersExp[1] += c[1];
        }
        
        var points = new List<int[]>
        {
            new[] { 0, (inputs.Length - 1) / 2 },
            new[] { inputs.Length - 1, (inputs.Length - 1) / 2 },
            new[] { (inputs.Length - 1) /2, inputs[0].Length - 1 },
            new[] { (inputs.Length - 1) /2, inputs[0].Length - 1 }
        };
        var countsPoints = new long[] { 0, 0 };
        foreach (var point in points)
        {
            var c = CountOnMap(inputs, inputs.Length - 1, new List<int[]> { point }, true);
            countsPoints[0] += c[0];
            countsPoints[1] += c[1];
        }

        var n = inputs.Length > 14 ? 202300 : 23;
        var count = Count(countsComplete, n, countsCorners, countsCornersExp, countsPoints, 1, 0);
        var countAlt = Count(countsComplete, n, countsCorners, countsCornersExp, countsPoints, 0, 1);
        Console.WriteLine($"Alt solution: {count}");
        return Task.FromResult(countAlt.ToString());
    }

    private static long Count(long[] countsComplete, int n, long[] countsCorners, long[] countsCornersExp, long[] countsPoints, int a, int b)
    {
        var countA = (countsComplete[a] * (n - 1) * (n - 1)) + (n-1) * countsCornersExp[a] + countsPoints[a];
        var countB = (countsComplete[b] * n * n) + (n * countsCorners[b]);
        var count = countA + countB;
        return count;
    }

    private long[] CountOnMap(char[][] inputs, int numSteps, List<int[]> startPositions, bool displayAtEnd = false)
    {
        var cardinalDirections = new List<int[]>
        {
            new[] { 0, 1 },
            new[] { 0, -1 },
            new[] { 1, 0 },
            new[] { -1, 0 },
        };

        var count = new long[] { 0, 0 };
        var startMap = GetMap(inputs);

        foreach (var pos in startPositions)
        {
            startMap[pos[0]][pos[1]] = '0';
            count[0]++;
        }

        for (var n = 0; n < numSteps; n++)
        {
            var isEven = (n + 1) % 2;
            var c = isEven == 0 ? '0' : '1';
            var notC = isEven == 0 ? '1' : '0';
            var nextOuterRim = new List<int[]>();
            foreach (var point in startPositions)
            {
                foreach (var direction in cardinalDirections)
                {
                    var checkY = point[0] + direction[0];
                    var checkX = point[1] + direction[1];

                    if (checkX >= 0 && checkY >= 0 && checkY < inputs.Length && checkX < inputs[checkY].Length &&
                        inputs[checkY][checkX] == notC)
                    {
                        Console.WriteLine($"Neighbours found ({point[0]}, {point[1]}) ({checkY}, {checkX})");
                    }

                    if (checkX >= 0 && checkY >= 0 && checkY < inputs.Length && checkX < startMap[checkY].Length &&
                        startMap[checkY][checkX] == '.')
                    {
                        startMap[checkY][checkX] = c;
                        count[isEven]++;
                        nextOuterRim.Add(new[] { checkY, checkX });
                    }
                }
            }

            startPositions = nextOuterRim;
        }

        if (displayAtEnd)
        {
            foreach (var line in startMap)
            {
                foreach (var character in line)
                {
                    Console.Write(character);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        return count;
    }

    private char[][] GetMap(char[][] inputs)
    {
        var newMap = inputs.Select(l => (char[])l.Clone()).ToArray();
        return newMap;
    }

    public string? TestInput => @".............
.........#.#.
.##........#.
....#........
....#...#....
.............
......S......
.............
..#.....#....
.......#.#...
.##.....#..#.
.##.......##.
.............";
}

internal class Map
{
    public char[][] Layout { get; set; }
}