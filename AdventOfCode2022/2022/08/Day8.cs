namespace AdventOfCode2022._2022._08;

public class Day8 : IDay
{
    public int Year => 2022;
    public int Day => 8;

    public string Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();

        var trees = inputs.Select(l => l.ToCharArray().Select(c => new Tree(int.Parse(c.ToString()))).ToArray())
            .ToArray();
        for (var x = 0; x < trees.Length; x++)
        {
            var highest = -1;
            for (var y = 0; y < trees[x].Length; y++)
            {
                if (trees[x][y].Height > highest)
                {
                    highest = trees[x][y].Height;
                    trees[x][y].IsVisible = true;
                }
            }
        }

        for (var y = 0; y < trees[0].Length; y++)
        {
            var highest = -1;
            for (var x = 0; x < trees.Length; x++)
            {
                if (trees[x][y].Height > highest)
                {
                    highest = trees[x][y].Height;
                    trees[x][y].IsVisible = true;
                }
            }
        }

        for (var x = trees.Length - 1; x >= 0; x--)
        {
            var highest = -1;
            for (var y = trees[x].Length - 1; y >= 0; y--)
            {
                if (trees[x][y].Height > highest)
                {
                    highest = trees[x][y].Height;
                    trees[x][y].IsVisible = true;
                }
            }
        }

        for (var y = trees[0].Length - 1; y >= 0; y--)
        {
            var highest = -1;
            for (var x = trees.Length - 1; x >= 0; x--)
            {
                if (trees[x][y].Height > highest)
                {
                    highest = trees[x][y].Height;
                    trees[x][y].IsVisible = true;
                }
            }
        }

        var result = trees.Sum(r => r.Count(t => t.IsVisible));
        Console.WriteLine(result);

        foreach (var line in trees)
        {
            Console.WriteLine(string.Join("", line.Select(l => l.IsVisible ? "Y" : "N")));
        }

        return result.ToString();
    }

    public string Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();

        var trees = inputs.Select(l => l.ToCharArray().Select(c => new Tree(int.Parse(c.ToString()))).ToArray())
            .ToArray();

        for (var x = 0; x < trees.Length; x++)
        {
            for (var y = 0; y < trees[x].Length; y++)
            {
                int lookUp = 0, lookDown = 0, lookLeft = 0, lookRight = 0;
                var i = x;
                while (i > 0)
                {
                    i--;
                    lookUp++;
                    if (trees[i][y].Height >= trees[x][y].Height) break;
                }

                i = x;
                while (i < trees.Length - 1)
                {
                    i++;
                    lookDown++;
                    if (trees[i][y].Height >= trees[x][y].Height) break;
                }

                var j = y;
                while (j > 0)
                {
                    j--;
                    lookLeft++;
                    if (trees[x][j].Height >= trees[x][y].Height) break;
                }

                j = y;
                while (j < trees[x].Length - 1)
                {
                    j++;
                    lookRight++;
                    if (trees[x][j].Height >= trees[x][y].Height) break;
                }

                Console.WriteLine($"{x},{y} : {lookUp} * {lookLeft} * {lookDown} * {lookRight}");
                trees[x][y].Score = lookLeft * lookRight * lookUp * lookDown;
            }
        }

        return trees.Max(l => l.Max(t => t.Score)).ToString();
    }

    public string TestInput => @"30373
25512
65332
33549
35390";

    public class Tree
    {
        public int Height { get; }

        public bool IsVisible { get; set; }
        public int Score { get; set; }

        public Tree(int height)
        {
            Height = height;
        }
    }
}