namespace AdventOfCode2023._2020._11;

public class Day11 : IDay
{
    public int Year => 2020;
    public int Day => 11;

    public string? Part1TestSolution => "37";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();

        var map = inputs;
        var changeMade = false;
        var t = 0;
        do
        {
            changeMade = false;
            t++;
            var nextMap = new char[map.Length][];
            for (var i = 0; i < nextMap.Length; i++)
            {
                nextMap[i] = new char[map[i].Length];
                for (var j = 0; j < map[i].Length; j++)
                {
                    var fulls = 0;
                    if (i > 0 && j > 0 && map[i - 1][j - 1] == '#')
                    {
                        fulls++;
                    }

                    if (i > 0 && map[i - 1][j] == '#')
                    {
                        fulls++;
                    }

                    if (i > 0 && j < map[i].Length - 1 &&  map[i - 1][j + 1] == '#')
                    {
                        fulls++;
                    }

                    if (j > 0 && map[i][j - 1] == '#')
                    {
                        fulls++;
                    }

                    if (j < map[i].Length - 1 && map[i][j + 1] == '#')
                    {
                        fulls++;
                    }

                    if (i < map.Length - 1 && j > 0 && map[i + 1][j - 1] == '#')
                    {
                        fulls++;
                    }

                    if (i < map.Length - 1 && map[i + 1][j] == '#')
                    {
                        fulls++;
                    }

                    if (i < map.Length - 1 && j < map[i].Length - 1 && map[i + 1][j + 1] == '#')
                    {
                        fulls++;
                    }

                    if (map[i][j] == 'L' && fulls == 0)
                    {
                        nextMap[i][j] = '#';
                        changeMade = true;
                    }
                    else if (map[i][j] == '#' && fulls >= 4)
                    {
                        nextMap[i][j] = 'L';
                        changeMade = true;
                    }
                    else
                    {
                        nextMap[i][j] = map[i][j];
                    }
                }
            }
            map = nextMap;
        } while (changeMade);

        return Task.FromResult(map.Sum(r => r.Count(c => c == '#')).ToString());
    }

    public string? Part2TestSolution => "26";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();

        var map = inputs;
        var changeMade = false;
        var t = 0;
        do
        {
            changeMade = false;
            t++;
            var nextMap = new char[map.Length][];
            for (var i = 0; i < nextMap.Length; i++)
            {
                nextMap[i] = new char[map[i].Length];
                for (var j = 0; j < map[i].Length; j++)
                {
                    var fulls = 0;
                    var shifts = new List<int[]>
                    {
                        new[] { -1, -1 },
                        new[] { -1, 0 },
                        new[] { -1, +1 },
                        new[] { 0, -1 },
                        new[] { 0, +1 },
                        new[] { +1, -1 },
                        new[] { +1, 0 },
                        new[] { +1, +1 }
                    };
                    foreach (var shift in shifts)
                    {
                        var k = i + shift[0];
                        var l = j + shift[1];
                        while (k >= 0 && l >= 0 && k < map.Length && l < map[k].Length)
                        {
                            if (map[k][l] == '#')
                            {
                                fulls++;
                                break;
                            }

                            if (map[k][l] == 'L')
                            {
                                break;
                            }

                            k+= shift[0];
                            l += shift[1];
                        }
                    }

                    if (map[i][j] == 'L' && fulls == 0)
                    {
                        nextMap[i][j] = '#';
                        changeMade = true;
                    }
                    else if (map[i][j] == '#' && fulls >= 5)
                    {
                        nextMap[i][j] = 'L';
                        changeMade = true;
                    }
                    else
                    {
                        nextMap[i][j] = map[i][j];
                    }
                }
            }
            map = nextMap;
        } while (changeMade);

        return Task.FromResult(map.Sum(r => r.Count(c => c == '#')).ToString());
    }

    public string? TestInput => @"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";
}