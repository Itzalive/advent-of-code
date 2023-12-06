namespace AdventOfCode2023._2022._14;

public class Day14 : IDay
{
    public int Year => 2022;
    public int Day => 14;

    public string? Part1TestSolution => null;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var lines = inputs.Select(l => l.Split(" -> ").Select(p => p.Split(",").Select(int.Parse).ToArray()).ToArray())
            .ToArray();
        var allPoints = lines.SelectMany(l => l).ToArray();
        var maxX = allPoints.Max(p => p[0]) + 1;
        var minX = allPoints.Min(p => p[0]) - 1;
        var maxY = allPoints.Max(p => p[1]) + 1;
        var minY = Math.Min(0, allPoints.Min(p => p[1]) - 1);
        var map = new List<List<char>>();
        for (var y = minY; y <= maxY; y++)
        {
            var row = new List<char>();
            for (var x = minX; x <= maxX; x++)
            {
                row.Add('.');
            }

            map.Add(row);
        }

        foreach (var line in lines)
        {
            var firstPoint = line[0];
            for (var i = 1; i < line.Length; i++)
            {
                var secondPoint = line[i];
                DrawLine(map, minX, minY, firstPoint, secondPoint);


                firstPoint = secondPoint;
            }
        }

        //PrintMap(map);

        var sandCount = 0;
        while (DropSand(map, minX, minY, 500, 0))
        {
            sandCount++;
        }

        Console.WriteLine();
        PrintMap(map);
        return Task.FromResult(sandCount.ToString());
    }

    private bool DropSand(List<List<char>> map, int mapMinX, int mapMinY, int sandXSpawn, int sandYSpawn)
    {
        var sandPoint = new int[] {sandXSpawn - mapMinX, sandYSpawn - mapMinY};

        if (map[sandPoint[1]][sandPoint[0]] == 'o')
            return false;

        bool hasMoved;
        do
        {
            hasMoved = false;
            if (sandPoint[1] == map.Count - 1)
                return false;
            if (map[sandPoint[1] + 1][sandPoint[0]] == '.')
            {
                sandPoint[1] += 1;
                hasMoved = true;
            }
            else if (map[sandPoint[1] + 1][sandPoint[0] - 1] == '.')
            {
                sandPoint[1] += 1;
                sandPoint[0] -= 1;
                hasMoved = true;
            }
            else if (map[sandPoint[1] + 1][sandPoint[0] + 1] == '.')
            {
                sandPoint[1] += 1;
                sandPoint[0] += 1;
                hasMoved = true;
            }
        } while (hasMoved);

        map[sandPoint[1]][sandPoint[0]] = 'o';
        return true;
    }

    private void PrintMap(List<List<char>> map)
    {
        foreach (var row in map)
        {
            Console.WriteLine(string.Join("", row));
        }
    }

    private void DrawLine(List<List<char>> map, int mapMinX, int mapMinY, int[] point, int[] point2)
    {
        var minX = Math.Min(point[0], point2[0]);
        var maxX = Math.Max(point[0], point2[0]);
        var minY = Math.Min(point[1], point2[1]);
        var maxY = Math.Max(point[1], point2[1]);
        for (var x = minX; x <= maxX; x++)
        {
            for (var y = minY; y <= maxY; y++)
            {
                map[y - mapMinY][x - mapMinX] = '#';
            }
        }
    }

    public string? Part2TestSolution => null;

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var lines = inputs.Select(l => l.Split(" -> ").Select(p => p.Split(",").Select(int.Parse).ToArray()).ToArray())
            .ToArray();
        var allPoints = lines.SelectMany(l => l).ToArray();
        var maxY = allPoints.Max(p => p[1]) + 2;
        var minY = Math.Min(0, allPoints.Min(p => p[1]) - 1);
        var maxX = 500 + (maxY - minY);
        var minX = 500 - (maxY - minY);
        var map = new List<List<char>>();
        for (var y = minY; y <= maxY; y++)
        {
            var row = new List<char>();
            for (var x = minX; x <= maxX; x++)
            {
                row.Add(y == maxY ? '#' : '.');
            }

            map.Add(row);
        }

        foreach (var line in lines)
        {
            var firstPoint = line[0];
            for (var i = 1; i < line.Length; i++)
            {
                var secondPoint = line[i];
                DrawLine(map, minX, minY, firstPoint, secondPoint);

                firstPoint = secondPoint;
            }
        }

        //PrintMap(map);

        var sandCount = 0;
        while (DropSand(map, minX, minY, 500, 0))
        {
            sandCount++;
        }

        Console.WriteLine();
        PrintMap(map);
        return Task.FromResult(sandCount.ToString());
    }

    public string TestInput => @"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9";
}