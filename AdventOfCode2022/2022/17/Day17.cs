using System.Linq;

namespace AdventOfCode2022._2022._17;

public class Day17 : IDay
{
    public int Year => 2022;

    public int Day => 17;
    
    const int xLength = 7;

    public string Part1(string input)
    {
        return DropRocks(input, 2022);
    }
    public string Part2(string input)
    {
        return DropRocks(input, 1000000000000);
    }

    private string DropRocks(string windDirections, long numRocks)
    {
        var inputs = windDirections.ToCharArray().Select(c => c == '<' ? -1 : 1).ToArray();
        var rocks = Rocks.Split(Environment.NewLine + Environment.NewLine)
            .Select(r => r.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray()).Select(r => new Rock(r)).ToArray();

        var cave = new List<int>(100000000);

        long removedLines = 0;
        var nextRock = 0;
        var nextWindIndex = 0;
        var towerTop = -1;
        int targetTowerTop = -1;
        var targetNextRockIndex = -1;
        var targetNextWindIndex = -1;
        var cycleDistance = 10;
        long cycleRocks = -1;
        long testCycleStart = -1;

        for (long i = 0; i < numRocks; i++)
        {
            // Place rock
            var rock = rocks[nextRock];
            var rockHeight = rock.Height;
            nextRock = (nextRock + 1) % rocks.Length;
            //PrintCave(cave);

            // Drop rock
            var startPosition = 2;
            var maxPosition = 7 - rock.Width;
            for (var w = 0; w < 4; w++)
            {
                var windDirection = GetNextWind(inputs, ref nextWindIndex);
                startPosition += windDirection;

                if (startPosition < 0)
                    startPosition = 0;
                else if (startPosition > maxPosition)
                    startPosition = maxPosition;
            }

            var byteRock = PlaceRock(rock, cave, towerTop, startPosition);
            var rockBottom = towerTop + 1;

            //PrintCave(cave);
            while (CanMoveRockDown(cave, rockBottom, byteRock))
            {
                rockBottom--;

                var direction = GetNextWind(inputs, ref nextWindIndex);
                BlowRock(cave, rockBottom, ref byteRock, direction);
            }

            HardenRock(cave, byteRock, rockBottom);
            towerTop = Math.Max(towerTop, rockBottom + rockHeight - 1);
            //if (i % 1000000 == 0) {
            //    var memorySavedLines = MemorySave(cave, towerTop);
            //    removedLines += memorySavedLines;
            //    towerTop -= memorySavedLines;
            //}

            if (towerTop > 1000 && targetTowerTop == -1)
            {
                for (var d = cycleDistance + 1; d < towerTop / 2; d++)
                {
                    var isMatch = true;
                    for (var j = 0; j < d; j++)
                    {
                        if (cave[towerTop - j] == cave[towerTop - d - j ]) continue;
                        isMatch = false;
                        break;
                    }

                    if (!isMatch) continue;

                    Console.WriteLine($"{i} {d} {towerTop}");
                    Console.WriteLine($"{i} {nextRock} {nextWindIndex}");
                    targetTowerTop = towerTop + d;
                    targetNextRockIndex = nextRock;
                    targetNextWindIndex = nextWindIndex;
                    testCycleStart = i;
                    cycleDistance = d;
                    break;
                }
                //Console.WriteLine(DateTime.UtcNow.ToString("O") + " - " + i);

            }

            if (targetTowerTop != -1 && towerTop >= targetTowerTop && cycleRocks == -1)
            {
                Console.WriteLine($"{i} {nextRock} {nextWindIndex}");

                if (towerTop == targetTowerTop && nextWindIndex == targetNextWindIndex && nextRock == targetNextRockIndex)
                {
                    cycleRocks = i - testCycleStart;
                    var remaining = numRocks - i - cycleDistance;
                    var timesCycled = (long) Math.Floor(remaining / (double) cycleRocks);
                    i += cycleRocks * timesCycled;
                    removedLines += cycleDistance * timesCycled;

                    Console.WriteLine($"{i} {nextRock} {nextWindIndex}");
                }
                
                if(towerTop > targetTowerTop)
                {
                    targetTowerTop = -1;
                }
            }

            //PrintCave(cave);
        }

        return (removedLines + cave.Count(l => l != 0)).ToString();
    }

    public double Correlation(int[] array1, int[] array2)
    {
        int[] array_xy = new int[array1.Length];
        int[] array_xp2 = new int[array1.Length];
        int[] array_yp2 = new int[array1.Length];
        for (int i = 0; i < array1.Length; i++)
            array_xy[i] = array1[i] * array2[i];
        for (int i = 0; i < array1.Length; i++)
            array_xp2[i] = (int)Math.Pow(array1[i], 2);
        for (int i = 0; i < array1.Length; i++)
            array_yp2[i] = (int)Math.Pow(array2[i], 2);
        var sum_x = array1.Sum();
        var sum_y = array2.Sum();
        var sum_xy = array_xy.Sum();
        var sum_xpow2 = array_xp2.Sum();
        var sum_ypow2 = array_yp2.Sum();
        var Ex2 = Math.Pow(sum_x, 2.00);
        var Ey2 = Math.Pow(sum_y, 2.00);

        return (array1.Length * sum_xy - sum_x * sum_y) /
               Math.Sqrt((array1.Length * sum_xpow2 - Ex2) * (array1.Length * sum_ypow2 - Ey2));
    }

    private static int GetNextWind(int[] inputs, ref int nextWindIndex)
    {
        var direction = inputs[nextWindIndex];
        nextWindIndex = (nextWindIndex + 1) % inputs.Length;
        return direction;
    }

    private int MemorySave(List<int> cave, int topOfTheRock)
    {
        var matched = 0;
        for (var y = topOfTheRock; y >= 0; y--)
        {
            var noneTrue = (matched & 1) == 0;
            for (var x = 0; x < xLength; x++)
            {
                if (((cave[y] >> x) & 1) == 1)
                {
                    matched |= 1 << x;
                    noneTrue = false;
                }
                else if (noneTrue)
                {
                    matched &= ~(1 << x);
                }
            }

            noneTrue = (matched & 64) == 0;
            for (var x = xLength - 1; x >= 0; x--)
            {
                if (((cave[y] >> x) & 1) == 1)
                {
                    matched |= 1 << x;
                    noneTrue = false;
                }
                else if (noneTrue)
                {
                    matched &= ~(1 << x);
                }
            }

            if (matched != 127) continue;
            cave.RemoveRange(0, y);
            return y;
        }

        return 0;
    }

    private void HardenRock(IList<int> cave, IReadOnlyList<int> rock, int rockBottom)
    {
        for (var y = rockBottom; y < rockBottom + rock.Count; y++)
        {
            cave[y] |= rock[y - rockBottom];
        }
    }

    private void PrintCave(List<int> cave)
    {
        var isPrinting = false;
        for (var y = cave.Count - 1; y >= 0; y--)
        {
            if (cave[y] != 0)
                isPrinting = true;
            if (isPrinting)
                Console.WriteLine("|" + new string(Convert.ToString(cave[y], toBase: 2).PadLeft(7, '.').Replace('1', '#').Replace('0', '.').Reverse().ToArray()) + "|");
        }

        Console.WriteLine("---------");
    }

    private void BlowRock(List<int> cave, int rockBottom, ref int[] rock, int windDirection)
    {
        if (windDirection > 0)
        {
            if (CanMoveRockLeft(cave, rockBottom, rock))
                MoveRockLeft(ref rock);
        }
        else
        {
            if (CanMoveRockRight(cave, rockBottom, rock))
                MoveRockRight(ref rock);
        }
    }

    private bool CanMoveRockDown(List<int> cave, int rockBottom, int[] rock)
    {
        if (rockBottom == 0) return false;
        for (var y = 0; y < rock.Length; y++)
        {
            var c = cave[rockBottom + y - 1];
            var t = rock[y];
            if ((c | t) != c + t) return false;
        }

        return true;
    }

    private void MoveRockLeft(ref int[] rock)
    {
        for (var i = 0; i < rock.Length; i++)
        {
            rock[i] <<= 1;
        }
    }

    private void MoveRockRight(ref int[] rock)
    {
        for (var i = 0; i < rock.Length; i++)
        {
            rock[i] >>= 1;
        }
    }

    private bool CanMoveRockLeft(List<int> cave, int rockBottom, int[] rock)
    {
        if (rock.Any(r => (r & 64) > 0)) return false;
        for (var y = 0; y < rock.Length; y++)
        {
            var c = cave[rockBottom + y];
            var t = rock[y] << 1;
            if ((c | t) != c + t) return false;
        }
        return true;
    }

    private bool CanMoveRockRight(List<int> cave, int rockBottom, int[] rock)
    {
        if (rock.Any(r => (r & 1) > 0)) return false;
        for (var y = 0; y < rock.Length; y++)
        {
            var c = cave[rockBottom + y];
            var t = rock[y] >> 1;
            if ((c | t) != c + t) return false;
        }
        return true;
    }

    private static int[] PlaceRock(Rock rock, List<int> cave, int towerTop, int xOffset)
    {
        while (towerTop + rock.Height >= cave.Count)
        {
            cave.Add(0);
        }

        var byteRock = rock.GetRock();

        for (var i = 0; i < byteRock.Length; i++)
            byteRock[i] <<= xOffset;

        return byteRock;
    }

    public string? TestInput => ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";

    private const string Rocks = @"####

.#.
###
.#.

..#
..#
###

#
#
#
#

##
##";
}

internal class Rock
{
    private readonly List<int> rock;

    public Rock(char[][] chars)
    {
        rock = new List<int>();
        Height = chars.Length;
        Width = chars[0].Length;
        foreach (var c in chars.Reverse())
        {
            var b = 0;
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == '#')
                    b |= (1 << i);
            }

            rock.Add(b);
        }
    }

    public int Width { get; set; }

    public int Height { get; set; }

    public int[] GetRock()
    {
        return rock.ToArray();
    }
}