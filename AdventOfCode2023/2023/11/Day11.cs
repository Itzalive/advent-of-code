namespace AdventOfCode2023._2023._11;

public class Day11 : IDay
{
    public int Year => 2023;
    public int Day => 11;

    public string? Part1TestSolution => "374";

    public Task<string> Part1(string input)
    {
        var score = Score(input, 2);

        return Task.FromResult(score.ToString());
    }

    private static long Score(string input, int sizeMultiplier)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var emptyRows = new List<int>();
        var colCount = new int[map[0].Length];
        var coordinates = new List<int[]>();
        for (var y = 0; y < map.Length; y++)
        {
            var rowIsEmpty = true;
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '#')
                {
                    colCount[x]++;
                    rowIsEmpty = false;
                    coordinates.Add(new[] { y, x });
                }
            }

            if (rowIsEmpty)
            {
                emptyRows.Add(y);
            }
        }

        var emptyCols = colCount.Select((v, i) => (v, i)).Where(pair => pair.v == 0).Select(pair => pair.i).ToArray();

        long score = 0;
        for (var i = 0; i < coordinates.Count; i++)
        {
            var coord1 = coordinates[i];
            for (var j = i + 1; j < coordinates.Count; j++)
            {
                var coord2 = coordinates[j];
                if (coord1 == coord2) continue;
                var distance = Math.Abs(coord1[1] - coord2[1]) + Math.Abs(coord1[0] - coord2[0]);
                var minY = Math.Min(coord1[0], coord2[0]);
                var maxY = Math.Max(coord1[0], coord2[0]);
                var minX = Math.Min(coord1[1], coord2[1]);
                var maxX = Math.Max(coord1[1], coord2[1]);
                var extraRows = emptyRows.Count(r => minY < r && r < maxY);
                var extraCols = emptyCols.Count(r => minX < r && r < maxX);
                score += (distance + (extraRows + extraCols) * (sizeMultiplier - 1));
            }
        }

        return score;
    }

    public string? Part2TestSolution => "1030";


    public Task<string> Part2(string input)
    {
        var score = Score(input, input.Split(Environment.NewLine).Count() < 20 ? 10 : 1000000);

        return Task.FromResult(score.ToString());
    }

    public string? TestInput => @"...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....";
}