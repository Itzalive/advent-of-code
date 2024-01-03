namespace AdventOfCode2023._2023._13;

public class Day13 : IDay
{
    public int Year => 2023;
    public int Day => 13;

    public string? Part1TestSolution => "405";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();

        var score = Score(inputs, 0);
        return Task.FromResult(score.ToString());
    }

    private static int Score(string[] inputs, int numSmudges)
    {
        var score = 0;
        foreach (var line in inputs)
        {
            var map = line.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
            for (var r = 0; r < map[0].Length - 1; r++)
            {
                var numDifferences = 0;
                for (var d = 0; d <= r && r + 1 + d < map[0].Length && numDifferences <= numSmudges; d++)
                {
                    for (var y = 0; y < map.Length && numDifferences <= numSmudges; y++)
                    {
                        if (map[y][r - d] == map[y][r + 1 + d]) continue;
                        numDifferences++;
                    }
                }

                if (numDifferences == numSmudges)
                {
                    score += r + 1;
                }
            }
        }

        foreach (var line in inputs)
        {
            var map = line.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
            for (var r = 0; r < map.Length - 1; r++)
            {
                var numDifferences = 0;
                for (var d = 0; d <= r && r + 1 + d < map.Length && numDifferences <= numSmudges; d++)
                {
                    for (var x = 0; x < map[0].Length && numDifferences <= numSmudges; x++)
                    {
                        if (map[r - d][x] == map[r + 1 + d][x]) continue;
                        numDifferences++;
                    }
                }


                if (numDifferences == numSmudges)
                {
                    score += 100 * (r + 1);
                }
            }
        }

        return score;
    }

    public string? Part2TestSolution => "400";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();

        var score = Score(inputs, 1);
        return Task.FromResult(score.ToString());
    }

    public string? TestInput => @"#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#";
}