namespace AdventOfCode2023._2024._25;

public class Day25 : IDay
{
    public int Year => 2024;

    public int Day => 25;

    public string? Part1TestSolution => "3";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine +Environment.NewLine).Select(l => l.Split(Environment.NewLine)).ToArray();
        var keys = new List<int[]>();
        var locks = new List<int[]>();
        foreach (var block in inputs)
        {
            if (block[0] == "#####")
            {
                var _lock = new int[5];
                for (var x = 0; x < 5; x++)
                {
                    for (var y = 1; y < 6; y++)
                    {
                        if (block[y][x] != '#')
                        {
                            _lock[x] = y - 1;
                            break;
                        }

                        if (y == 5)
                        {
                            _lock[x] = 5;
                        }
                    }
                }

                locks.Add(_lock);
            }
            else
            {
                var key = new int[5];
                for (var x = 0; x < 5; x++)
                {
                    for (var y = 5; y > 0; y--)
                    {
                        if (block[y][x] != '#')
                        {
                            key[x] = 5 - y;
                            break;
                        }

                        if (y == 1)
                        {
                            key[x] = 5;
                        }
                    }
                }

                keys.Add(key);
            }
        }

        var score = 0;
        foreach (var key in keys)
        {
            foreach(var _lock in locks)
            {
                var isMatch = true;
                for (var i = 0; i < 5; i++)
                {
                    if (key[i] + _lock[i] > 5)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    score++;
                }
            }
        }

        return Task.FromResult(score.ToString());
    }

    public string? Part2TestSolution => null;


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        return Task.FromResult("");
    }

    public string? TestInput => @"#####
.####
.####
.####
.#.#.
.#...
.....

#####
##.##
.#.##
...##
...#.
...#.
.....

.....
#....
#....
#...#
#.#.#
#.###
#####

.....
.....
#.#..
###..
###.#
###.#
#####

.....
.....
.....
#....
#.#..
#.#.#
#####";
}