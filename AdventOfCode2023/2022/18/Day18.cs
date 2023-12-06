namespace AdventOfCode2023._2022._18;

public class Day18 : IDay
{
    public int Year => 2022;

    public int Day => 18;

    public string? Part1TestSolution => null;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var cubes = inputs.Select(i => i.Split(",").Select(int.Parse).ToArray()).ToArray();
        var maxX = cubes.Max(c => c[0]) + 3;
        var maxY = cubes.Max(c => c[1]) + 3;
        var maxZ = cubes.Max(c => c[2]) + 3;
        var area = new char[maxX, maxY, maxZ];
        foreach (var c in cubes)
        {
            area[c[0] + 1, c[1] + 1, c[2] + 1] = 'c';
        }

        var count = 0;
        var cubeDiffs = new int[][]
            {new[] {-1, 0, 0}, new[] {1, 0, 0}, new[] {0, -1, 0}, new[] {0, 1, 0}, new[] {0, 0, -1}, new[] {0, 0, 1}};
        foreach (var c in cubes)
        {
            foreach (var cubeDiff in cubeDiffs)
            {
                if (area[c[0] + cubeDiff[0] + 1, c[1] + cubeDiff[1] + 1, c[2] + cubeDiff[2] + 1] != 'c')
                {
                    count++;
                }
            }
        }

        return Task.FromResult(count.ToString());
    }

    public string? Part2TestSolution => null;

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var cubes = inputs.Select(i => i.Split(",").Select(int.Parse).ToArray()).ToArray();
        var maxX = cubes.Max(c => c[0]) + 3;
        var maxY = cubes.Max(c => c[1]) + 3;
        var maxZ = cubes.Max(c => c[2]) + 3;
        var area = new char[maxX, maxY, maxZ];
        foreach (var c in cubes)
        {
            area[c[0] + 1, c[1] + 1, c[2] + 1] = 'c';
        }

        var pointsToFillFrom = new Queue<int[]>();
        pointsToFillFrom.Enqueue(new[] {0, 0, 0});

        var count = 0;
        var cubeDiffs = new int[][]
            {new[] {-1, 0, 0}, new[] {1, 0, 0}, new[] {0, -1, 0}, new[] {0, 1, 0}, new[] {0, 0, -1}, new[] {0, 0, 1}};
        while (pointsToFillFrom.Count > 0)
        {
            var point = pointsToFillFrom.Dequeue();
            foreach (var cubeDiff in cubeDiffs)
            {
                var x = point[0] + cubeDiff[0];
                var y = point[1] + cubeDiff[1];
                var z = point[2] + cubeDiff[2];
                if (x < 0 || x >= maxX) continue;
                if (y < 0 || y >= maxY) continue;
                if (z < 0 || z >= maxZ) continue;
                if (area[x, y, z] == 'c')
                {
                    count++;
                    continue;
                }

                if(area[x, y, z] != 'w')
                {
                    area[x, y, z] = 'w';
                    pointsToFillFrom.Enqueue(new[] {x, y, z});
                }
            }
        }

        return Task.FromResult(count.ToString());
    }

    public string? TestInput => @"2,2,2
1,2,2
3,2,2
2,1,2
2,3,2
2,2,1
2,2,3
2,2,4
2,2,6
1,2,5
3,2,5
2,1,5
2,3,5";
}