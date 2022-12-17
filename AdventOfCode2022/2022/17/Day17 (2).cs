﻿namespace AdventOfCode2022._2022._17;

public class Day17b : IDay
{
    public int Year => 2022;

    public int Day => 117;

    public string Part1(string input)
    {
        var inputs = input.ToCharArray();
        var rocks = Rocks.Split(Environment.NewLine + Environment.NewLine)
            .Select(r => r.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray()).ToArray();

        var cave = new List<char[]>(200);

        long removedLines = 0;
        var nextRock = 0;
        var nextWind = 0;
        var topOfTheRock = -1;
        for (var i = 0; i < 2022; i++)
        {
            // Place rock
            var rock = rocks[nextRock];
            var rockHeight = rock.Length;
            topOfTheRock = PlaceRock(rock, cave, topOfTheRock, 2);
            nextRock = (nextRock + 1) % rocks.Length;
            //PrintCave(cave);

            // Drop rock
            var totalWind = 0;
            for (var w = 0; w < 4; w++)
            {
                var windDirection = GetNextWind(inputs, ref nextWind);
                if (windDirection == '<')
                    totalWind--;
                else
                    totalWind++;

                if (totalWind < -2)
                    totalWind = -2;
                else if (totalWind > (5 - rock[0].Length))
                    totalWind = (5 - rock[0].Length);
            }

            switch (totalWind)
            {
                case < 0:
                    MoveRockLeft(cave, topOfTheRock, rockHeight, totalWind);
                    break;
                case > 0:
                    MoveRockRight(cave, topOfTheRock, rockHeight, totalWind);
                    break;
            }
            //PrintCave(cave);
            var movedDown = 0;
            while (CanMoveRockDown(cave, topOfTheRock, rockHeight))
            {
                MoveRockDown(cave, topOfTheRock, rockHeight);
                topOfTheRock--;
                movedDown++;
                
                var direction = GetNextWind(inputs, ref nextWind);
                BlowRock(cave, topOfTheRock, rockHeight, direction);
            }

            HardenRock(cave, topOfTheRock, rockHeight);
            topOfTheRock += Math.Max(movedDown - rockHeight, 0);
            var memorySavedLines = MemorySave(cave, topOfTheRock);
            removedLines += memorySavedLines;
            topOfTheRock -= memorySavedLines;
            //Console.WriteLine(topOfTheRock);
            //PrintCave(cave);
        }

        return (removedLines + cave.Count(l => l.Any(c => c != '.'))).ToString();
    }

    private static char GetNextWind(char[] inputs, ref int nextWind)
    {
        var direction = inputs[nextWind];
        nextWind = (nextWind + 1) % inputs.Length;
        return direction;
    }

    private int MemorySave(List<char[]> cave, int topOfTheRock)
    {
        var matched = new bool[cave[0].Length];
        for (var y = topOfTheRock; y >= 0; y--)
        {
            var noneTrue = !matched[0];
            for (var x = 0; x < cave[y].Length; x++)
            {
                if (cave[y][x] == '#')
                {
                    matched[x] = true;
                    noneTrue = false;
                }
                else if (noneTrue)
                {
                    matched[x] = false;
                }
            }

            noneTrue = !matched[^1];
            for (var x = cave[y].Length - 1; x >= 0; x--)
            {
                if (cave[y][x] == '#')
                {
                    matched[x] = true;
                    noneTrue = false;
                }
                else if (noneTrue)
                {
                    matched[x] = false;
                }
            }

            if (!matched.All(m => m)) continue;
            cave.RemoveRange(0, y);
            return y;
        }

        return 0;
    }

    private void HardenRock(List<char[]> cave, int topOfTheRock, int rockHeight)
    {
        for (var y = topOfTheRock - rockHeight + 1; y <= topOfTheRock; y++)
        {
            for (var x = 0; x < cave[y].Length; x++)
            {
                if (cave[y][x] != '@') continue;
                cave[y][x] = '#';
            }
        }
    }

    private void PrintCave(List<char[]> cave)
    {
        var isPrinting = false;
        for (var y = cave.Count - 1; y >= 0; y--)
        {
            if (cave[y].Any(c => c != '.'))
                isPrinting = true;
            if (isPrinting)
                Console.WriteLine("|" + string.Join("", cave[y]) + "|");
        }

        Console.WriteLine("---------");
    }

    private void BlowRock(List<char[]> cave, int topOfTheRock, int rockHeight, char direction)
    {
        if (direction == '<')
        {
            if (CanMoveRockLeft(cave, topOfTheRock, rockHeight))
                MoveRockLeft(cave, topOfTheRock, rockHeight);
        }
        else
        {
            if (CanMoveRockRight(cave, topOfTheRock, rockHeight))
                MoveRockRight(cave, topOfTheRock, rockHeight);
        }
    }

    private void MoveRockDown(List<char[]> cave, int topOfTheRock, int rockHeight)
    {
        var maxY = Math.Min(topOfTheRock + 1, cave.Count);
        var minY = Math.Max(0, topOfTheRock - rockHeight  + 1);

        var xLength = cave[0].Length;
        for (var y = minY; y < maxY; y++)
        {
            for (var x = 0; x < xLength; x++)
            {
                if (cave[y][x] != '@') continue;
                cave[y - 1][x] = '@';
                cave[y][x] = '.';
            }
        }
    }

    private bool CanMoveRockDown(List<char[]> cave, int topOfTheRock, int rockHeight)
    {
        var maxY = Math.Min(topOfTheRock + 1, cave.Count);
        var minY = Math.Max(0, topOfTheRock - rockHeight  + 1);
        
        var xLength = cave[0].Length;
        for (var x = 0; x < xLength; x++)
        {
            for (var y = minY; y < maxY; y++)
            {
                if (cave[y][x] != '@') continue;
                if (y == 0 || cave[y - 1][x] == '#') return false;
                break;
            }
        }

        return true;
    }

    private void MoveRockLeft(List<char[]> cave, int topOfTheRock, int rockHeight, int distance = -1)
    {
        var maxY = Math.Min(topOfTheRock + 1, cave.Count);
        var minY = Math.Max(0, topOfTheRock - rockHeight  + 1);
        
        var xLength = cave[0].Length;
        for (var y = minY; y < maxY; y++)
        {
            for (var x = 0; x < xLength; x++)
            {
                if (cave[y][x] != '@') continue;
                cave[y][x + distance] = '@';
                cave[y][x] = '.';
            }
        }
    }

    private void MoveRockRight(List<char[]> cave, int topOfTheRock, int rockHeight, int distance = 1)
    {
        var maxY = Math.Min(topOfTheRock + 1, cave.Count);
        var minY = Math.Max(0, topOfTheRock - rockHeight  + 1);

        var xStart = cave[0].Length - 1;
        for (var y = minY; y < maxY; y++)
        {
            for (var x = xStart; x >= 0; x--)
            {
                if (cave[y][x] != '@') continue;
                cave[y][x + distance] = '@';
                cave[y][x] = '.';
            }
        }
    }

    private bool CanMoveRockLeft(List<char[]> cave, int topOfTheRock, int rockHeight)
    {
        var maxY = Math.Min(topOfTheRock + 1, cave.Count);
        var minY = Math.Max(0, topOfTheRock - rockHeight  + 1);
        
        var xLength = cave[0].Length;
        for (var y = minY; y < maxY; y++)
        {
            for (var x = 0; x < xLength; x++)
            {
                if (cave[y][x] != '@') continue;
                if (x == 0 || cave[y][x - 1] == '#') return false;
                break;
            }
        }

        return true;
    }

    private bool CanMoveRockRight(List<char[]> cave, int topOfTheRock, int rockHeight)
    {
        var maxY = Math.Min(topOfTheRock + 1, cave.Count);
        var minY = Math.Max(0, topOfTheRock - rockHeight  + 1);
        
        var startX = cave[0].Length - 1;
        for (var y = minY; y < maxY; y++)
        {
            for (var x = startX; x >= 0; x--)
            {
                if (cave[y][x] != '@') continue;
                if (x == cave[y].Length - 1 || cave[y][x + 1] == '#') return false;
                break;
            }
        }

        return true;
    }

    private static int PlaceRock(char[][] rock, List<char[]> cave, int topOfTheRock, int xOffset)
    {
        while (topOfTheRock + rock.Length >= cave.Count)
        {
            cave.Add(new[] {'.', '.', '.', '.', '.', '.', '.'});
        }

        for (var h = 1; h <= rock.Length; h++)
        {
            for (var rx = 0; rx < rock[0].Length; rx++)
            {
                cave[topOfTheRock + h][rx + xOffset] = rock[^h][rx] == '#' ? '@' : '.';
            }
        }

        return topOfTheRock + rock.Length;
    }


    public string Part2(string input)
    {
        var inputs = input.ToCharArray();
        var rocks = Rocks.Split(Environment.NewLine + Environment.NewLine)
            .Select(r => r.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray()).ToArray();

        var cave = Enumerable.Range(0, 100).Select(i => new[] {'.', '.', '.', '.', '.', '.', '.'}).ToList();

        long removedLines = 0;
        var nextRock = 0;
        var nextWind = 0;
        var topOfTheRock = -1;
        for (long i = 0; i < 1000000000000; i++)
        {
            // Place rock
            var rock = rocks[nextRock];
            var rockHeight = rock.Length;
            nextRock = (nextRock + 1) % rocks.Length;
            //PrintCave(cave);

            // Drop rock
            var totalWind = 0;
            for (var w = 0; w < 4; w++)
            {
                var windDirection = GetNextWind(inputs, ref nextWind);
                if (windDirection == '<')
                    totalWind--;
                else
                    totalWind++;

                if (totalWind < -2)
                    totalWind = -2;
                else if (totalWind > (5 - rock[0].Length))
                    totalWind = (5 - rock[0].Length);
            }

            
            topOfTheRock = PlaceRock(rock, cave, topOfTheRock, 2 + totalWind);

            //PrintCave(cave);
            var movedDown = 0;
            while (CanMoveRockDown(cave, topOfTheRock, rockHeight))
            {
                MoveRockDown(cave, topOfTheRock, rockHeight);
                topOfTheRock--;
                movedDown++;
                
                var direction = GetNextWind(inputs, ref nextWind);
                BlowRock(cave, topOfTheRock, rockHeight, direction);
            }

            HardenRock(cave, topOfTheRock, rockHeight);
            topOfTheRock += Math.Max(movedDown - rockHeight, 0);
            if (i % 10000000 == 0)
            {
                var memorySavedLines = MemorySave(cave, topOfTheRock);
                removedLines += memorySavedLines;
                topOfTheRock -= memorySavedLines;
            }

            if (i % 1000000 == 0)
            {
                Console.WriteLine(DateTime.UtcNow.ToString("O") + " - " + i);
            }
        }

        return (removedLines + cave.Count(l => l.Any(c => c != '.'))).ToString();
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