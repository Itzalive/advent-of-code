namespace AdventOfCode2023._2024._12;

public class Day12 : IDay
{
    public int Year => 2024;

    public int Day => 12;

    public string? Part1TestSolution => "1930";

    public Task<string> Part1(string input)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var checkedMap = Enumerable.Range(0, map.Length)
            .Select(_ => Enumerable.Range(0, map[0].Length).Select(_ => -1).ToArray()).ToArray();

        var shape = 0;
        var seedPos = findNextStartPos(checkedMap);
        var score = 0;
        var directions = new List<(int X, int Y)>
        {
            (0, 1),
            (1, 0),
            (-1, 0),
            (0, -1)
        };
        do
        {
            var area = 0;
            var perimeter = 0;
            var positionsToCheck = new Queue<(int X, int Y)>();
            positionsToCheck.Enqueue((seedPos.Value.X, seedPos.Value.Y));
            while (positionsToCheck.Any())
            {
                var checking = positionsToCheck.Dequeue();

                if (checkedMap[checking.Y][checking.X] != -1) continue;

                checkedMap[checking.Y][checking.X] = shape;
                area++;
                foreach (var direction in directions)
                {
                    var newX = checking.X + direction.X;
                    var newY = checking.Y + direction.Y;
                    if (newX < 0 || newY < 0 || newY >= map.Length || newX >= map[0].Length)
                    {
                        perimeter++;
                    }
                    else if (map[newY][newX] != map[seedPos.Value.Y][seedPos.Value.X])
                    {
                        perimeter++;
                    }
                    else if (checkedMap[newY][newX] == -1)
                    {
                        positionsToCheck.Enqueue((newX, newY));
                    }
                }
            }
            Console.WriteLine($"{(char)(shape + (int)'A')}: {perimeter} x {area}");
            shape++;
            score += perimeter * area;
            seedPos = findNextStartPos(checkedMap);
        } while (seedPos != null);

        foreach (var line in checkedMap)
        {
            Console.WriteLine(string.Join("", line.Select(v => (char)(v + (int)'A'))));
        }

        return Task.FromResult(score.ToString());
    }

    private (int X, int Y)? findNextStartPos(int[][] checkedMap)
    {
        for (var y = 0; y < checkedMap.Length; y++)
        {
            for (var x = 0; x < checkedMap[y].Length; x++)
            {
                if (checkedMap[y][x] == -1) return (x, y);
            }
        }
        return null;
    }

    public string? Part2TestSolution => "1206";


    public Task<string> Part2(string input)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var checkedMap = Enumerable.Range(0, map.Length)
            .Select(_ => Enumerable.Range(0, map[0].Length).Select(_ => -1).ToArray()).ToArray();

        var shape = 0;
        var seedPos = findNextStartPos(checkedMap);
        var score = 0;
        var directions = new List<(int X, int Y)>
        {
            (0, 1),
            (1, 0),
            (-1, 0),
            (0, -1)
        };
        var cornersDirections = new List<(int X, int Y)[]>
        {
            new []{(1, 0), (1,1), (0,1)},
            new []{(1, 0),(1, -1), (0, -1)},
                new []{(-1, 0),(-1, 1), (0, 1)},
                    new []{(-1, 0), (-1, -1), (0, -1)}
        };
        do
        {
            var area = 0;
            var corners = 0;
            var positionsToCheck = new Queue<(int X, int Y)>();
            positionsToCheck.Enqueue((seedPos.Value.X, seedPos.Value.Y));
            while (positionsToCheck.Any())
            {
                var checking = positionsToCheck.Dequeue();

                if (checkedMap[checking.Y][checking.X] != -1) continue;

                checkedMap[checking.Y][checking.X] = shape;
                area++;
                var edges = 0;
                foreach (var direction in directions)
                {
                    var newX = checking.X + direction.X;
                    var newY = checking.Y + direction.Y;
                    if (newX < 0 || newY < 0 || newY >= map.Length || newX >= map[0].Length)
                    {
                        edges++;
                    }
                    else if (map[newY][newX] != map[seedPos.Value.Y][seedPos.Value.X])
                    {
                        edges++;
                    }
                    else if (checkedMap[newY][newX] == -1)
                    {
                        positionsToCheck.Enqueue((newX, newY));
                    }
                }

                foreach (var cornerPoints in cornersDirections)
                {
                    var diagonalY = checking.Y + cornerPoints[1].Y;
                    var diagonalX = checking.X + cornerPoints[1].X;
                    var diagonalIsNotMatch = diagonalX < 0 || diagonalY < 0 || diagonalY >= map.Length ||
                                             diagonalX >= map[0].Length || map[diagonalY][diagonalX] !=
                                             map[seedPos.Value.Y][seedPos.Value.X];

                    var leftY = checking.Y + cornerPoints[0].Y;
                    var leftX = checking.X + cornerPoints[0].X;
                    var rightY = checking.Y + cornerPoints[2].Y;
                    var rightX = checking.X + cornerPoints[2].X;
                    if ((leftX < 0 || leftY < 0 || leftY >= map.Length || leftX >= map[0].Length || map[leftY][leftX] != map[seedPos.Value.Y][seedPos.Value.X])
                        && (rightX < 0 || rightY < 0 || rightY >= map.Length || rightX >= map[0].Length || map[rightY][rightX] != map[seedPos.Value.Y][seedPos.Value.X]))
                    {
                        corners++;
                    }
                    else if (diagonalIsNotMatch && leftX >= 0 && leftY >= 0 && leftY < map.Length && leftX < map[0].Length && map[leftY][leftX] ==
                             map[seedPos.Value.Y][seedPos.Value.X] &&
                             rightX >= 0 && rightY >= 0 && rightY < map.Length && rightX < map[0].Length &&
                             map[rightY][rightX] ==
                             map[seedPos.Value.Y][seedPos.Value.X])
                    {
                        corners++;
                    }
                }
            }
            Console.WriteLine($"{(char)(shape + (int)'A')}: {area} x {corners}");
            shape++;
            score += corners * area;
            seedPos = findNextStartPos(checkedMap);
        } while (seedPos != null);

        foreach (var line in checkedMap)
        {
            Console.WriteLine(string.Join("", line.Select(v => (char)(v + (int)'A'))));
        }

        return Task.FromResult(score.ToString());
    }

    public string? TestInput => @"RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE";
}