namespace AdventOfCode2023._2022._12;

public class Day12 : IDay
{
    public int Year => 2022;
    public int Day => 12;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        Point currentPoint = null;
        var grid = new List<List<Point>>();
        for (int x = 0; x < inputs.Length; x++)
        {
            var row = new List<Point>();
            var line = inputs[x].ToCharArray();
            for (int y = 0; y < line.Length; y++)
            {
                var point = new Point(x, y, line[y]);
                row.Add(point);
                if (point.IsStarting)
                {
                    currentPoint = point;
                }
            }

            grid.Add(row);
        }

        var result = MoveToFinish(grid, currentPoint);
        Console.WriteLine(result.WasSuccess);
        Console.WriteLine(result.Path.Count - 1);
        Console.WriteLine();
        foreach (var row in grid)
        {
            Console.WriteLine(string.Join(" ",
                row.Select(r =>
                    ((char) (r.Height + (int) 'a')) + (r.FastestHere > 999 ? "XXX" : r.FastestHere.ToString("000")))));
        }

        return Task.FromResult((result.Path.Count - 1).ToString());
    }

    public static PathResult MoveToFinish(List<List<Point>> grid, Point startingPoint)
    {
        var nextToCheck = new List<(List<Point>, Point)> {(new List<Point>(), startingPoint)};
        List<Point> bestPath = null;
        while (nextToCheck.Count > 0)
        {
            var pathToCheck = nextToCheck.MaxBy(p => p.Item2.Height);
            nextToCheck.Remove(pathToCheck);
            var existingPath = pathToCheck.Item1;
            var nextPoint = pathToCheck.Item2;
            if (nextPoint.FastestHere <= existingPath.Count + 1) continue;
            if (existingPath.Contains(nextPoint)) continue;
            if (existingPath.Any())
            {
                var latestPoint = existingPath.Last();
                if (nextPoint.Height - latestPoint.Height > 1) continue;
            }

            var newPath = new List<Point>(existingPath) {nextPoint};
            nextPoint.FastestHere = newPath.Count;
            if (nextPoint.IsFinishing)
            {
                if (bestPath == null || bestPath.Count > newPath.Count)
                    bestPath = newPath;
            }

            if (nextPoint.X > 0)
            {
                nextToCheck.Add((newPath, grid[nextPoint.X - 1][nextPoint.Y]));
            }

            if (nextPoint.X < grid.Count - 1)
            {
                nextToCheck.Add((newPath, grid[nextPoint.X + 1][nextPoint.Y]));
            }

            if (nextPoint.Y > 0)
            {
                nextToCheck.Add((newPath, grid[nextPoint.X][nextPoint.Y - 1]));
            }

            if (nextPoint.Y < grid[0].Count - 1)
            {
                nextToCheck.Add((newPath, grid[nextPoint.X][nextPoint.Y + 1]));
            }
        }

        return new PathResult {WasSuccess = bestPath != null, Path = bestPath ?? new List<Point>()};
    }

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        Point currentPoint = null;
        var grid = new List<List<Point>>();
        for (int x = 0; x < inputs.Length; x++)
        {
            var row = new List<Point>();
            var line = inputs[x].ToCharArray();
            for (int y = 0; y < line.Length; y++)
            {
                var point = new Point(x, y, line[y]);
                row.Add(point);
                if (point.IsStarting)
                {
                    currentPoint = point;
                }
            }

            grid.Add(row);
        }

        var aPoints = grid.SelectMany(r => r.Where(p => p.Height == 0)).ToArray();
        var results = new List<PathResult>();
        foreach (var aPoint in aPoints)
        {
            var moveToFinish = MoveToFinish(grid, aPoint);
            if (moveToFinish.WasSuccess)
                results.Add(moveToFinish);
        }

        var result = results.OrderBy(r => r.Path.Count).First();
        Console.WriteLine(result.WasSuccess);
        Console.WriteLine(result.Path.Count - 1);
        Console.WriteLine();

        return Task.FromResult((result.Path.Count - 1).ToString());
    }

    public class Point
    {
        public int X { get; }

        public int Y { get; }

        public int Height { get; set; }

        public bool IsStarting { get; set; }

        public bool IsFinishing { get; set; }

        public int FastestHere { get; set; } = int.MaxValue;

        public Point(int x, int y, char height)
        {
            X = x;
            Y = y;
            IsStarting = height == 'S';
            IsFinishing = height == 'E';
            Height = height == 'S' ? 0 : height == 'E' ? 25 : (int) height - (int) 'a' + 0;
        }
    }

    public class PathResult
    {
        public bool WasSuccess { get; set; }

        public List<Point> Path { get; set; } = new();
    }

    public string TestInput => @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi";
}