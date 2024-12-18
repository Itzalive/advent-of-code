namespace AdventOfCode2023._2024._10;

public class Day10 : IDay
{
    public int Year => 2024;

    public int Day => 10;

    public string? Part1TestSolution => "36";

    public Task<string> Part1(string input)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray().Select(v => int.Parse(v.ToString())).ToArray())
            .ToArray();
        var trailheads = new List<(int X, int Y)>();
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == 0)
                {
                    trailheads.Add((x, y));
                }
            }
        }

        var score = 0;
        var changes = new List<(int X, int Y)>
        {
            (0, 1),
            (1, 0),
            (-1, 0),
            (0, -1)
        };
        foreach (var trailhead in trailheads)
        {
            var positions = new List<(int X, int Y)> { trailhead };
            for (var height = 1; height < 10; height++)
            {
                var nextPositions = new List<(int X, int Y)>();
                foreach (var position in positions)
                {
                    foreach (var change in changes)
                    {
                        var newX = position.X + change.X;
                        var newY = position.Y + change.Y;
                        if (newX < 0 || newX >= map[0].Length || newY < 0 || newY >= map.Length) continue;
                        if (map[newY][newX] != height) continue;
                        nextPositions.Add((newX, newY));
                    }
                }

                positions = nextPositions.Distinct().ToList();
            }

            score += positions.Count;
        }

        return Task.FromResult(score.ToString());
    }

    public string? Part2TestSolution => "81";


    public Task<string> Part2(string input)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray().Select(v => int.Parse(v.ToString())).ToArray())
            .ToArray();
        var trailheads = new List<(int X, int Y)>();
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == 0)
                {
                    trailheads.Add((x, y));
                }
            }
        }

        var score = 0;
        var changes = new List<(int X, int Y)>
        {
            (0, 1),
            (1, 0),
            (-1, 0),
            (0, -1)
        };
        foreach (var trailhead in trailheads)
        {
            var positions = new List<(int X, int Y, int Count)> { (trailhead.X, trailhead.Y, 1) };
            for (var height = 1; height < 10; height++)
            {
                var nextPositions = new List<(int X, int Y, int Count)>();
                foreach (var position in positions)
                {
                    foreach (var change in changes)
                    {
                        var newX = position.X + change.X;
                        var newY = position.Y + change.Y;
                        if (newX < 0 || newX >= map[0].Length || newY < 0 || newY >= map.Length) continue;
                        if (map[newY][newX] != height) continue;
                        nextPositions.Add((newX, newY, position.Count));
                    }
                }

                positions = nextPositions.GroupBy(v => (v.X, v.Y)).Select(g => (g.Key.X, g.Key.Y, g.Sum(v => v.Count)))
                    .ToList();
            }

            score += positions.Sum(v => v.Count);
        }

        return Task.FromResult(score.ToString());
    }

    public string? TestInput => @"89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732";
}